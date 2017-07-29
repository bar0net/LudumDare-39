using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {
    public enum DamageTypes { LASER, PROJECTILE }

    public GameObject projectile;
    public Enemy target = null;
    public float cooldown = 0.1f;
    public int damage = 25;
    public DamageTypes type;

    float timer = 0f;
    Vector3 shootingPoint;
    int bulletCost;
    bool active = true;

    GameManager _gm;
    public Color currColor = Color.white;

	// Use this for initialization
	void Start () {
        _gm = FindObjectOfType<GameManager>();

        shootingPoint = new Vector3(this.transform.GetChild(0).position.x, this.transform.GetChild(0).position.y, 0);
        bulletCost = projectile.GetComponent<Projectile>().bulletCost;
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
        //target.Hit(damage);

        switch(type)
        {
            case DamageTypes.LASER:
                Laser();
                break;

            case DamageTypes.PROJECTILE:
                ShootProjectile();
                break;
        }
    }

    void Laser()
    {
        Debug.DrawLine(shootingPoint, target.transform.position);

        LineRenderer lr = GetComponent<LineRenderer>();
        if (GetComponent<LineRenderer>() == null)
        {
            lr = this.gameObject.AddComponent<LineRenderer>();
            
        }
        //lr.material = mat;
        //lr.startWidth = 0.01f;
        //lr.endWidth = 0.01f;
        lr.SetPosition(0, shootingPoint);            
        lr.SetPosition(1, target.transform.position);

        
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

    private void OnPostRender()
    {
        /*
        if (target != null)
        {
            Debug.Log("Test");
            //GL.LoadOrtho();
            GL.Begin(GL.LINES);
            mat.SetPass(0);
            GL.Color(Color.red);
            GL.Vertex(shootingPoint);
            GL.Vertex(target.transform.position);
            GL.End();
        }
        */
        
    }

    public void ToogleTurret ()
    {
        active = !active;

        if (active) currColor = Color.white;
        else currColor = new Color(1.0f,0.5f,0.5f);

        GetComponent<SpriteRenderer>().color = currColor;
    }
}
