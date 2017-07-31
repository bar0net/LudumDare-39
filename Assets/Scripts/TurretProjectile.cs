using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretProjectile : Turret {
    public GameObject projectile;
    public GameObject target;
    public float cooldown = 0.1f;
    int bulletCost = 0;

    float timer = 0;
    Transform shootPoint;
	// Use this for initialization
	void Start () {
        if (projectile == null) this.enabled = false;

        bulletCost = projectile.GetComponent<Projectile>().bulletCost;

        shootPoint = this.transform.Find("ShootingPoint");
        if (shootPoint == null) this.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (!isActive) return; 

        if (timer < 0)
        {
            if (target != null) Shoot();
        }
        else timer -= Time.deltaTime;
	}

    void Shoot()
    {
        if (_gm.currPower < bulletCost) return;

        GameObject bullet = (GameObject)Instantiate(projectile, shootPoint.position, Quaternion.identity);
        Projectile p = bullet.GetComponent<Projectile>();
        p.target = target.transform;
        if (this.transform.position.y < target.transform.position.y) p.behindTower = true;
        _gm.DrainPower(bulletCost);

        timer = cooldown;
    }
}
