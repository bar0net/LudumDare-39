using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {
    [System.Serializable]
    public struct Wave
    {
        public GameObject enemy;
        public GameObject pathing;
        public int number;
        public float delay;
        public float wait;
    }

    [SerializeField]
    public Wave[] waves;
    
    float timer = 2.0f;
    int currWave = 0;

    GameObject enemies;

	// Use this for initialization
	void Start () {
        enemies = new GameObject("Enemies");
	}
	
	// Update is called once per frame
	void Update () {
        if (currWave >= waves.Length) return;

        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            SpawnEnemy();

            if (waves[currWave].number <= 0)
            {
                timer = waves[currWave].wait;
                currWave++;
            }
            else timer = waves[currWave].delay;
        }
	}

    void SpawnEnemy()
    {
        Transform tr = waves[currWave].pathing.transform.GetChild(0);

        GameObject go = (GameObject)Instantiate(waves[currWave].enemy, tr.transform.position, Quaternion.identity, enemies.transform);
        go.GetComponent<Enemy>().pathing = waves[currWave].pathing;
        go.GetComponent<SpriteRenderer>().sortingOrder = waves[currWave].number;
        go.name = "Enemy_" + waves[currWave].number + "_" + currWave;

        waves[currWave].number--;
    }
}
