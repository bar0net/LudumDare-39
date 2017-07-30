using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {
    public float damage = 1;
    public float cost = 1;
    public bool isActive = true;
    public Color currColor = Color.white;

    protected GameManager _gm;

    private void Awake()
    {
        _gm = FindObjectOfType<GameManager>();
    }

    public void ToogleTurret()
    {
        isActive = !isActive;

        if (isActive) currColor = Color.white;
        else currColor = new Color(1.0f, 0.5f, 0.5f);

        GetComponent<SpriteRenderer>().color = currColor;
    }
}
