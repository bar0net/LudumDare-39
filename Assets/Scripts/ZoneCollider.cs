using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneCollider : MonoBehaviour {

    TurretProjectile _t;
    List<GameObject> listEnemies = new List<GameObject>();
    
    private void Start()
    {
        _t = this.transform.parent.GetComponent<TurretProjectile>();
    }

    private void Update()
    {
        if (listEnemies.Count > 0 && _t.target == null) _t.target = listEnemies[0];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            listEnemies.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (_t.target != null && _t.target.name == collision.gameObject.name) _t.target = null;

            if (listEnemies.Count > 0) listEnemies.Remove(collision.gameObject);
        }
    }
}
