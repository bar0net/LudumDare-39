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

    GameManager _gm;

	// Use this for initialization
	void Start () {
        enemies = new GameObject("Enemies");
        _gm = FindObjectOfType<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
        if (currWave >= waves.Length) return;

        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            if (waves[currWave].enemy == null && enemies.transform.childCount <= 0)
            {
                EndOfNight();
                return;
            }
            else if (waves[currWave].enemy != null) SpawnEnemy();

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
        go.name = "Enemy_" + waves[currWave].number + "_" + currWave;

        SpriteRenderer[] _srs = go.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer _sr in _srs) _sr.sortingOrder = waves[currWave].number;

        waves[currWave].number--;
    }

    void EndOfNight()
    {
        timer = waves[currWave].wait;
        currWave++;

        if (currWave >= waves.Length) EndGame();

        _gm.ChangeDaytime(false);
    }

    void EndGame()
    {
        _gm.GameOver(true);
    }
}
