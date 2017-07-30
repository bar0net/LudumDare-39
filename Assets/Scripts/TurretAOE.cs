using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAOE : Turret {

    public float damage = 1f;
    public float cost = 0.1f;



    public List<Enemy> enemies = new List<Enemy>();

    float accCost = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (enemies.Count > 0)
        {
            for (int i = 0; i < enemies.Count; ++i) enemies[i].Hit(damage * Time.deltaTime);
        }
	}
}
