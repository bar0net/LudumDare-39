using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

    public GameObject turret = null;

    GameManager _gm;
    List<SpriteRenderer> _sr = new List<SpriteRenderer>();

	// Use this for initialization
	void Start () {
        _gm = FindObjectOfType<GameManager>();
        _sr.Add( this.GetComponent<SpriteRenderer>() );

        _sr[0].sortingOrder =  - (int)this.transform.position.y;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseEnter()
    {
        if ((turret == null && _gm.towerToBuild != null) || (turret != null && _gm.toogleingPower))
        {
            for (int i = 0; i < _sr.Count; ++i) _sr[i].color = new Color(0.75f, 0.75f, 0.75f);
        }
        
    }

    private void OnMouseOver()
    {
    }

    private void OnMouseExit()
    {
        if ((turret == null && _gm.towerToBuild != null) || (turret != null && _gm.toogleingPower))
        {
            for (int i = 0; i < _sr.Count; ++i) _sr[i].color = Color.white;
        }

    }

    private void OnMouseUp()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (_gm.towerToBuild != null) BuildTurret();
            if (_gm.toogleingPower && turret != null)
            {
                turret.GetComponent<Turret>().ToogleTurret();
                _gm.ChangeTooglePower();
                _sr[0].color = Color.white;
            }
        }
    }

    private void BuildTurret()
    {
        GameObject go = (GameObject)Instantiate(
            _gm.towerToBuild,
            this.transform.position - 1.5f*Vector3.up + Vector3.forward,
            Quaternion.identity,
            this.transform);
        turret = go;

        _gm.SetTowerToBuild(-1);
        _sr[0].color = Color.white;

        _sr.Add(go.GetComponent<SpriteRenderer>());
    }
}
