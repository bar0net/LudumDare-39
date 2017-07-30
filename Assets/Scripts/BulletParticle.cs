using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletParticle : MonoBehaviour {
    public float lifetime = 0.5f;
    float timer;
	// Use this for initialization
	void Start () {
        timer = lifetime;
	}
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;
        this.transform.localScale = Vector3.one * timer / lifetime;

        if (timer <= 0) Destroy(this.gameObject);
	}
}
