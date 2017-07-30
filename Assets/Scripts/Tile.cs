using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

    public GameObject turret = null;
    Turret _t = null; 

    GameManager _gm;
    List<SpriteRenderer> _sr = new List<SpriteRenderer>();

	// Use this for initialization
	void Start () {
        _gm = FindObjectOfType<GameManager>();
        _sr.Add( this.GetComponent<SpriteRenderer>() );

        _sr[0].sortingOrder =  - (int)this.transform.position.y;
        _sr[0].color = Color.white;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseEnter()
    {
        if ((turret == null && _gm.towerToBuild != null) || (turret != null && _gm.toogleingPower && !(_t is TurretSolar)) )
        {
            for (int i = 0; i < _sr.Count; ++i) _sr[i].color =  new Color(0.75f *_sr[i].color.r, 0.75f * _sr[i].color.g, 0.75f * _sr[i].color.b, 1.0f); //new Color(0.75f, 0.75f, 0.75f);
        }
        
    }

    private void OnMouseOver()
    {
    }

    private void OnMouseExit()
    {
        if ((turret == null && _gm.towerToBuild != null) || (turret != null && _gm.toogleingPower && !(_t is TurretSolar)))
        {
            _sr[0].color = Color.white;
            if (turret != null) _sr[1].color = _t.currColor;
        }

    }

    private void OnMouseUp()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (_gm.towerToBuild != null) BuildTurret();
            if (_gm.toogleingPower && turret != null && !(_t is TurretSolar))
            {
                _t.ToogleTurret();
                //_gm.ChangeTooglePower();
                //_sr[0].color = Color.white;
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
        _t = go.GetComponent<Turret>();
    }
}
