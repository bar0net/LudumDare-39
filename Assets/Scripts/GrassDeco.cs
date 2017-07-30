using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassDeco : MonoBehaviour {
    public Sprite[] decorations;


	// Use this for initialization
	void Start () {
        if (Random.value < 0.33)
        {
            GetComponent<SpriteRenderer>().sprite = decorations[Random.Range(0, decorations.Length)];
        }
        else Destroy(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
