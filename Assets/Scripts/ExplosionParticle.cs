using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionParticle : MonoBehaviour {
    public float time = 0.5f;
    public Color color = Color.white;
    public Vector3 inputDir = Vector3.zero;
    Vector3 dir;
    public float velocity = 0.5f;

    float timer = 0;
	// Use this for initialization
	void Start () {
        GetComponent<SpriteRenderer>().color = color;
        dir = (new Vector3(Random.value-0.5f, Random.value-0.5f, 0) + inputDir).normalized;
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.unscaledDeltaTime;

        if (timer > time) Destroy(this.gameObject);

        this.transform.position += dir * Time.fixedUnscaledDeltaTime * velocity;
        float value = Mathf.Lerp(1f, 0.001f, timer / time);
        this.transform.localScale = new Vector3(value,value,1);
	}
}
