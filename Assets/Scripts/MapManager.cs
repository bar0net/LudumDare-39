using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {
    public Vector2 mapSize = new Vector2(20, 20);

    public GameObject[] Tiles;
    public GameObject discardTile;

    int[,] map;

    List<Position> waypoints = new List<Position>();

    float water_level = 0.30f;
    float grass_level = 0.50f;

    protected struct Position
    {
        public int x;
        public int y;

        public Position(int _x, int _y)
        {
            this.x = _x;
            this.y = _y;
        }

        public Position Left() { return new Position(x - 1, y); }
        public Position Right() { return new Position(x + 1, y); }
        public Position Up() { return new Position(x, y - 1); }
        public Position Down() { return new Position(x, y + 1); }
    }

    // Use this for initialization
	void Start () {
        int seed = PlayerPrefs.GetInt("seed", (int)(Random.value * Time.deltaTime * 1000000000));

        PlayerPrefs.SetInt("seed", seed);
        Random.InitState(seed);

        water_level += 0.1f * Random.value;
        grass_level += 0.1f * Random.value;

        map = new int[(int)mapSize.x, (int)mapSize.y];
        for (int i = 0; i < mapSize.x; ++i)
        {
            for (int j = 0; j < mapSize.y; ++j)
            {
                if (Mathf.Abs(i-10) + Mathf.Abs(j-10) <= 10) map[i, j] = -1;
                else map[i, j] = -2;
            }
        }
        int value = Random.Range(0, 10);
        Position pos = new Position(value, 10-value);

        // Create Road
        while (pos.y + pos.x < 30)
        {
            Position next = PlaceRoad(pos);

            if (next.x == pos.x && next.y == pos.y) break;
            else pos = next;
        }
        map[pos.x, pos.y] = 1;

        // Create fields
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

        float norm = maximum - minimum;
        float water_norm = minimum + water_level * norm;
        float grass_norm = maximum - grass_level * norm;

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


    List<Position> ViableNeighbors(Position pos)
    {
        List<Position> output = new List<Position>();

        if (pos.y > 0 && map[pos.x, pos.y-1] == -1) output.Add(pos.Up());
        if (pos.x < mapSize.x - 1 && map[pos.x + 1, pos.y] == -1) output.Add(pos.Right());
        if (pos.y < mapSize.y - 1 && map[pos.x, pos.y+1] == -1) output.Add(pos.Down());

        return output;
    }

    Position PlaceRoad(Position pos)
    {
        map[pos.x, pos.y] = 1;
        waypoints.Add(pos);

        List<Position> neighbors = ViableNeighbors(pos);

        if (neighbors.Count > 0)
        {
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

    void PlaceWaypoints()
    {
        GameObject path = new GameObject();

        GameObject point = new GameObject();
        point.transform.position = Position2World(waypoints[0]);
        point.transform.SetParent(path.transform);

        for (int i = 1; i < waypoints.Count - 1; ++i)
        {
            if (Diff(waypoints[i - 1], waypoints[i]) == Diff(waypoints[i], waypoints[i + 1])) continue;

            point = new GameObject();
            point.transform.position = Position2World(waypoints[i]);
            point.transform.SetParent(path.transform);
        }

        point = new GameObject();
        point.transform.position = Position2World(waypoints[waypoints.Count-1]);
        point.transform.SetParent(path.transform);

        SpawnManager sm = FindObjectOfType<SpawnManager>();
        for (int i = 0; i < sm.waves.Length; ++i) sm.waves[i].pathing = path;
    }

    Vector3 Position2World(Position pos)
    {
        return new Vector3(2 * pos.x - 2 * pos.y, -pos.x - pos.y + 20, 0);

    }

    Vector2 Diff(Position p1, Position p2) { return new Vector2(p2.x - p1.x, p2.y - p1.y); }
}
