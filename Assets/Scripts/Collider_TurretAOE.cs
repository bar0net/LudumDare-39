using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collider_TurretAOE : MonoBehaviour {

    TurretAOE _t;

    private void Start()
    {
        _t = this.transform.parent.GetComponent<TurretAOE>();

        // If this collider is on a terrain tile and not a path
        // we don't need it, so destroy it!
        Collider2D col = Physics2D.OverlapPoint(this.transform.position, LayerMask.NameToLayer("Terrain"));
        if (col != null && col.tag == "Terrain") Destroy(this.gameObject);
    }


    // Add Enemies to this building's target list when they enter the collider
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            _t.AddTarget(collision.gameObject.GetComponent<Enemy>());
        }
    }

    // Remove enemies from this building's target list when they exit the collider
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            _t.RemoveTarget(collision.gameObject.GetComponent<Enemy>());
        }
    }
}
