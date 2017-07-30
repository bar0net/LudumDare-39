using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeBar : MonoBehaviour {
    public Enemy _self;
    public GameObject bar;

    SpriteRenderer[] sr;
    int last;

	// Use this for initialization
	void Start () {
        last = Mathf.FloorToInt(_self.health);
        sr = new SpriteRenderer[last];

        for (int i = 0; i < last; ++i)
        {
            GameObject go = (GameObject)Instantiate(bar, this.transform);

            go.transform.localPosition = new Vector3( -0.5f + (float)i / (float)last, 0, 0);
            go.transform.localScale = new Vector3((float)1 / (float)(last + 1), 1, 1);
            sr[i] = go.GetComponent<SpriteRenderer>();
            sr[i].color = Color.green;
        }

	}
	
	// Update is called once per frame
	void Update () {

        if  ((int) _self.health < last)
        {
            sr[last - 1].color = Color.red;
            last--;
        }
		
	}
}
