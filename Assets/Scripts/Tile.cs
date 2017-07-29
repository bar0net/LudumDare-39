using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

    public GameObject turret = null;

    GameManager _gm;
    SpriteRenderer _sr;

	// Use this for initialization
	void Start () {
        _gm = FindObjectOfType<GameManager>();
        _sr = this.GetComponent<SpriteRenderer>();

        _sr.sortingOrder =  - (int)this.transform.position.y;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseEnter()
    {
        if (turret == null && _gm.towerToBuild != null)
            _sr.color = new Color(0.9f, 0.9f, 0.9f);
        
    }

    private void OnMouseOver()
    {
    }

    private void OnMouseExit()
    {
        if (turret == null)
            _sr.color = Color.white;

    }

    private void OnMouseUp()
    {
        if (_gm.towerToBuild != null)
        {
            BuildTurret();
        }
    }

    private void BuildTurret()
    {
        GameObject go = (GameObject)Instantiate(
            _gm.towerToBuild,
            this.transform.position - 1.5f*Vector3.up,
            Quaternion.identity,
            this.transform);
        turret = go;

        _gm.SetTowerToBuild(-1);
        _sr.color = Color.white;
    }
}
