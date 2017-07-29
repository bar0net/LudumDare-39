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

	// Use this for initialization
	void Start () {
        shootingPoint = this.transform.GetChild(0).position;
	}
	
	// Update is called once per frame
	void Update () {
        if (timer > 0) timer -= Time.deltaTime;
        else
        {
            if (target != null)
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
        GameObject go = (GameObject)Instantiate(projectile, shootingPoint, Quaternion.identity);
        go.GetComponent<Projectile>().target = target.transform;
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
}
