using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAOE : Turret {

    public List<Enemy> enemies = new List<Enemy>();

    [SerializeField]
    float accCost = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!isActive || _gm.currPower <= 0) return;
        if (!_gm.isNight) return;

		if (enemies.Count > 0)
        {
            for (int i = 0; i < enemies.Count; ++i) enemies[i].Hit(damage * Time.deltaTime);
        }

        accCost += cost * Time.deltaTime;
        if (accCost > 1)
        {
            int power = Mathf.FloorToInt(accCost);

            _gm.DrainPower(power);
            accCost -= power;
        }
	}
}
