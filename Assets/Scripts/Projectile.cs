using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    public Transform target;
    public bool follow = true;
    public int damage = 25;
    public float velocity = 10f;
    public bool checkTimer = true;
    public bool checkDistance = true;
    public bool behindTower = false;

    float timer = 0;
	// Use this for initialization
	void Start () {
        if (checkTimer) timer = (target.transform.position - this.transform.position).magnitude / velocity;

        if (behindTower) this.GetComponent<SpriteRenderer>().sortingLayerName = "Bullet-Back";
    }
	
	// Update is called once per frame
	void Update () {
        if (target == null)
        {
            Destroy(this.gameObject);
            return;
        }

        if (checkTimer && timer <= 0)
        {
            Hit();
            return;
        }

        Vector3 v = target.transform.position - this.transform.position;

        if (v.magnitude < GameManager._BULLET_PROXIMITY_)
        {
            Hit();
            return;
        }

        this.transform.position += v.normalized * Time.deltaTime * velocity;

	}

    void Hit()
    {
        target.gameObject.GetComponent<Enemy>().Hit(damage);
        Destroy(this.gameObject);

    }
}
