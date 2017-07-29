using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

	// Use this for initialization
	void Start () {
        this.GetComponent<SpriteRenderer>().sortingOrder =  - (int)this.transform.position.y;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
