using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAOE : Turret {

    public List<Enemy> target = new List<Enemy>();
    bool childrenDisabled = false;

    public float powerConsumtion = 0.4f;

    [SerializeField]
    float accCost = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!isActive || _gm.currPower <= 0 || !_gm.isNight)
        {
            if (!childrenDisabled)
            {
                for (int i = 0; i < this.transform.childCount; ++i) this.transform.GetChild(i).gameObject.SetActive(false);
                childrenDisabled = true;
            }
            return;
        }

        if (childrenDisabled)
        {
            for (int i = 0; i < this.transform.childCount; ++i) this.transform.GetChild(i).gameObject.SetActive(true);
            childrenDisabled = false;
        }

		if (target.Count > 0)
        {
            for (int i = 0; i < target.Count; ++i) target[i].Hit(damage * Time.deltaTime);
        }

        accCost += powerConsumtion * Time.deltaTime;
        if (accCost > 1)
        {
            int power = Mathf.FloorToInt(accCost);

            _gm.DrainPower(power);
            accCost -= power;
        }
	}

    public void AddTarget(Enemy enemy) {
        if (!target.Contains(enemy)) target.Add(enemy);
    }

    public void RemoveTarget(Enemy enemy)
    {
        target.Remove(enemy);
    }
}
