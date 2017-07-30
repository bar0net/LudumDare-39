using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {
    public enum DamageTypes { LASER, PROJECTILE, SOLAR }

    public GameObject projectile;
    public Enemy target = null;
    public float cooldown = 0.1f;
    public int damage = 25;
    public DamageTypes type;

    float timer = 0f;
    Vector3 shootingPoint;
    public int bulletCost = 0;
    bool active = true;

    GameManager _gm;
    public Color currColor = Color.white;

	// Use this for initialization
	void Start () {
        _gm = FindObjectOfType<GameManager>();

        if (this.transform.childCount > 0)
            shootingPoint = new Vector3(this.transform.GetChild(0).position.x, this.transform.GetChild(0).position.y, 0);

        if (projectile != null)
            bulletCost = projectile.GetComponent<Projectile>().bulletCost;

        if (type == DamageTypes.SOLAR) _gm.ExpandMaxPower(damage);
	}
	
	// Update is called once per frame
	void Update () {
        if (timer > 0) timer -= Time.deltaTime;
        else
        {
            if (target != null && active)
            {
                Shoot();
                timer = cooldown;
            }
        }
	}

    void Shoot()
    {
        switch(type)
        {
            case DamageTypes.PROJECTILE:
                ShootProjectile();
                break;
        }
    }

    void ShootProjectile()
    {
        if (_gm.currPower < bulletCost) return;

        GameObject go = (GameObject)Instantiate(projectile, shootingPoint, Quaternion.identity);
        Projectile p = go.GetComponent<Projectile>();
        p.target = target.transform;
        if (this.transform.position.y < target.transform.position.y) p.behindTower = true;
        _gm.DrainPower(bulletCost);
    }

    public void ToogleTurret ()
    {
        active = !active;

        if (active) currColor = Color.white;
        else currColor = new Color(1.0f,0.5f,0.5f);

        GetComponent<SpriteRenderer>().color = currColor;
    }
}
