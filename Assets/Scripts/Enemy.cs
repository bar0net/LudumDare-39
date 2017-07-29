using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public GameObject pathing;
    public int health = 100;
    public float velocity = 0.5f;

    [SerializeField]
    int next_waypoint = 0;

    [SerializeField]
    Vector3 direction;

    Vector3 offset;

    float timer;
    
	// Use this for initialization
	void Start () {
        if (pathing == null) Destroy(this.gameObject);
        offset = new Vector3(Random.Range(-0.2f,0.2f), Random.Range(-0.2f, 0.2f), 0);
	}
	
	// Update is called once per frame
	void Update () {
        if (timer < 0) GetNextWaypoint();

        this.transform.position += direction * velocity * Time.deltaTime;
        timer -= Time.deltaTime;
	}

    void GetNextWaypoint()
    {
        next_waypoint++;

        if (next_waypoint < pathing.transform.childCount)
        {
            direction = pathing.transform.GetChild(next_waypoint).position + offset - this.transform.position;
            timer = direction.magnitude / velocity;
            direction = direction.normalized;
        }
        else Success();

    }

    void Success()
    {
        Destroy(this.gameObject);
    }

    void Die()
    {
        Destroy(this.gameObject);
    }

    public void Hit(int damage)
    {
        health -= damage;

        if (health <= 0) Die();
    }

}
