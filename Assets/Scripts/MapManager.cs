using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {
    // Defines the size of the map in a convinient vector
    public Vector2 mapSize = new Vector2(20, 20);

    // List of tiles to use when drawing the map
    public GameObject[] Tiles;
    
    // Tile to use if the map points to a non-existing
    // tile from Tiles (also useful to debug mid map generation)
    public GameObject discardTile;

    // Design variables to stablish the [0,1] threshold
    // that determines if a tile is water or grass ( or sand)
    float water_level = 0.30f;
    float grass_level = 0.50f;

    /* 
    // TODO: Maybe create a Tile class/struct that contains
    // the game object and the threshold.
    // This was literally implemented in the last hour of the compo,
    // so time to be practical
    */

    // the map is represented as a matrix of tiles
    // at the end, each cell should contain the index of the
    // tile from Tiles to display
    int[,] map;

    // Note: I am using a square matrix to define a diamond shape worldspace
    // that diamond is defined by: 
    // abs(y) < 0.5 * height - abs(x) * height / width

    // List of path waypoints that the enemies will follow
    List<Position> waypoints = new List<Position>();

    // Define a position tuple
    protected struct Position
    {
        public int x;
        public int y;

        public Position(int _x, int _y)
        {
            this.x = _x;
            this.y = _y;
        }

        // Set of methods to return a Position next to the current one
        public Position Left() { return new Position(x - 1, y); }
        public Position Right() { return new Position(x + 1, y); }
        public Position Up() { return new Position(x, y - 1); }
        public Position Down() { return new Position(x, y + 1); }
    }

    // Use this for initialization
	void Start () {
         
        // Load the seed for the random generator if it is stroed in player prefs
        // or generate a new one. 
        // I store the seed to be able to reproduce the level if the player chooses
        // to restart it.
        //
        // TODO: Maybe use something more elegant to decide a new seed
        int seed = PlayerPrefs.GetInt("seed", (int)(Random.value * Time.deltaTime * 1000000000));

        PlayerPrefs.SetInt("seed", seed);
        Random.InitState(seed);


        // Add some random variation to the thresholds (probably unnecessary)
        water_level += 0.1f * Random.value;
        grass_level += 0.1f * Random.value;

        // Create and populate the map with -1 as the default value
        map = new int[(int)mapSize.x, (int)mapSize.y];
        for (int i = 0; i < mapSize.x; ++i)
        {
            for (int j = 0; j < mapSize.y; ++j)
            {
                if (Mathf.Abs(i-10) + Mathf.Abs(j-10) <= 10) map[i, j] = -1;
                else map[i, j] = -2;
            }
        }

        // Get a random starting location
        int value = Random.Range(0, 10);
        Position pos = new Position(value, 10-value);

        // Keep creating a path until it reaches the other side
        // y + x < 30 is a derivation of using a diamond shape world
        // inside a square matrix
        while (pos.y + pos.x < 30)
        {
            Position next = PlaceRoad(pos);

            // if PlaceRoads return the same position, something has gone wrong
            // It shouldn't happen though.
            if (next.x == pos.x && next.y == pos.y) break;
            else pos = next;
        }
        // Make sure we have placed the last bit of path
        map[pos.x, pos.y] = 1;

        // Populate the rest of the map with other types of tile
        // To have some semblance of cohesion without spending too much time on 
        // creating smart policies, I will be using Perlin Noise.

        // first I will create an auxiliary map with the Perlin Noise values
        // and check some statistical data (minimum and maximum values)
        float[,] mounts = new float[20, 20];
        float minimum = 1.0f;
        float maximum = 0.0f;

        for (int i = 0; i < mapSize.x; ++i)
        {
            for (int j = 0; j < mapSize.y; ++j)
            {
                mounts[i,j] = Mathf.PerlinNoise(Random.value * 10 + Random.value * i, Random.value * 10 + Random.value * j);

                if (mounts[i, j] < minimum) minimum = mounts[i, j];
                if (mounts[i, j] > maximum) maximum = mounts[i, j];
            }
        }

        // Redefine the tile thresholds based on the statistical data
        // of the new "elevation" map
        float norm = maximum - minimum;
        float water_norm = minimum + water_level * norm;
        float grass_norm = maximum - grass_level * norm;

        // Populate "map" with the appropiate tile indexes 
        for (int i = 0; i < mapSize.x; ++i)
        {
            for (int j = 0; j < mapSize.y; ++j)
            {
                if (map[i, j] != -1) continue;

                if (mounts[i, j] < water_norm) map[i, j] = 2;
                else if (mounts[i, j] > grass_norm) map[i, j] = 0;
                else map[i, j] = -1;
            }
        }

        DrawMap();
        PlaceWaypoints();
    }

    // Given a valid position, return the list of valid neighbors
    // valid neighbors are positions within the bounds of the matrix
    // that still have the default value in map
    List<Position> ViableNeighbors(Position pos)
    {
        List<Position> output = new List<Position>();

        if (pos.y > 0 && map[pos.x, pos.y-1] == -1) output.Add(pos.Up());
        if (pos.x < mapSize.x - 1 && map[pos.x + 1, pos.y] == -1) output.Add(pos.Right());
        if (pos.y < mapSize.y - 1 && map[pos.x, pos.y+1] == -1) output.Add(pos.Down());

        return output;
    }

    // Place a path segment
    Position PlaceRoad(Position pos)
    {
        // Place the segment and add the position to the waypoints list
        // for later use
        map[pos.x, pos.y] = 1;
        waypoints.Add(pos);

        // Get the list of all valid next positions
        List<Position> neighbors = ViableNeighbors(pos);


        if (neighbors.Count > 0)
        {
            // Get a random neighbor as the next path position
            // and place grass tiles to the rest
            int next = Random.Range(0, neighbors.Count);
            pos = neighbors[next];

            for (int i = 0; i < neighbors.Count; ++i)
            {
                if (i == next) continue;
                map[neighbors[i].x, neighbors[i].y] = 0;
            }
            
        }
        return pos;


    }

    // Create a tile for each map cell
    // Note: we need to perform a base change from matrix coordinates to world coordinates
    // Matrix(1,0) -> World(2,-1)
    // Matrix(0,1) -> World(-2,-1)

    // TODO: Adapt Position2World method and use it to define the new vector for clarity  
    void DrawMap() {
        for (int j = 0; j < mapSize.x; ++j)
        {
            for (int i = 0; i < mapSize.y; ++i)
            {
                if (map[i,j] <= -1)
                {
                    GameObject go = (GameObject)Instantiate(discardTile, new Vector3(2*i - 2*j, -i - j + 20, -1), Quaternion.identity);
                    go.name = i.ToString() + " " + j.ToString();
                }
                else
                {
                    GameObject go = (GameObject)Instantiate(Tiles[map[i,j]], new Vector3(2 * i - 2*j, - i - j + 20, -1), Quaternion.identity);
                    go.name = i.ToString() + " " + j.ToString();
                }
            }
        }
    }

    // Create the path gameobject that the enemies will use to 
    // define their movement pattern
    void PlaceWaypoints()
    {
        GameObject path = new GameObject();

        // Place the first enemy-waypoint
        GameObject point = new GameObject();
        point.transform.position = Position2World(waypoints[0]);
        point.transform.SetParent(path.transform);

        // Create a new enemy-waypoint on corners (if the direction
        // between a pair of points change respect the last pair)
        for (int i = 1; i < waypoints.Count - 1; ++i)
        {
            if (Diff(waypoints[i - 1], waypoints[i]) == Diff(waypoints[i], waypoints[i + 1])) continue;

            point = new GameObject();
            point.transform.position = Position2World(waypoints[i]);
            point.transform.SetParent(path.transform);
        }

        // place the last enemy-waypoint
        point = new GameObject();
        point.transform.position = Position2World(waypoints[waypoints.Count-1]);
        point.transform.SetParent(path.transform);

        // Assign the path to all waves of enemies in SpawnManager
        SpawnManager sm = FindObjectOfType<SpawnManager>();
        for (int i = 0; i < sm.waves.Length; ++i) sm.waves[i].pathing = path;
    }


    // Note: we need to perform a base change from matrix coordinates to world coordinates
    // Matrix(1,0) -> World(2,-1)
    // Matrix(0,1) -> World(-2,-1)
    Vector3 Position2World(Position pos)
    {
        return new Vector3(2 * pos.x - 2 * pos.y, -pos.x - pos.y + 20, 0);

    }

    Vector2 Diff(Position p1, Position p2) { return new Vector2(p2.x - p1.x, p2.y - p1.y); }
}
