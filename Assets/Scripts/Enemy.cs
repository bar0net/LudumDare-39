using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public GameObject pathing;
    public float health = 10f;
    public float velocity = 0.5f;

    public int loot = 10; 

    [SerializeField]
    int next_waypoint = 0;

    [SerializeField]
    Vector3 direction;

    Vector3 offset;

    float timer;
    bool mirrored = false;

    public Vector2 maxOffset = new Vector2(0.2f, 0.2f);
    
	// Use this for initialization
	void Start () {
        if (pathing == null) Destroy(this.gameObject);
        offset = new Vector3(Random.Range(-maxOffset.x, maxOffset.x), Random.Range(-maxOffset.y, maxOffset.y), 0);

        Animator anim = this.GetComponentInChildren<Animator>();
        if (anim != null)
        {
            AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
            anim.Play(state.fullPathHash, -1, Random.Range(0f, 1f));
        }
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

            if ((direction.x > 0 && !mirrored) || (direction.x < 0 && mirrored))
            {
                mirrored = !mirrored;
                this.transform.localScale = new Vector3(-this.transform.localScale.x, 1, 1);
                Transform _child_tr = this.transform.GetChild(0).transform;
                _child_tr.localPosition = new Vector3(-_child_tr.localPosition.x, _child_tr.localPosition.y, _child_tr.localPosition.z);
            }
            
        }
        else Success();

    }

    void Success()
    {
        FindObjectOfType<GameManager>().KillPopulation(Mathf.CeilToInt(health));
        Destroy(this.gameObject);
    }

    void Die()
    {
        FindObjectOfType<GameManager>().EarnMoney(loot);
        Destroy(this.gameObject);
    }

    public void Hit(int damage)
    {
        health -= damage;

        if (health <= 0) Die();
    }

    public void Hit(float damage)
    {
        health -= damage;

        if (health <= 0) Die();
    }

}
