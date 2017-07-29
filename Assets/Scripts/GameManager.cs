using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public const float _BULLET_PROXIMITY_ = 0.2f;

    public GameObject[] towerTypes;

    public GameObject towerToBuild { private set; get; }

    SpawnManager _sm;
    Animator _ambient_anim;
	// Use this for initialization
	void Start () {
        towerToBuild = null;

        _sm = FindObjectOfType<SpawnManager>();
        _sm.enabled = false;

        _ambient_anim = GameObject.Find("Ambient").GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void ChangeTimeScale(float scale)
    {
        Time.timeScale = scale;
    }

    public void SetTowerToBuild(int index)
    {
        if (index == -1) towerToBuild = null;
        else if (index != -1 && index < towerTypes.Length) towerToBuild = towerTypes[index];
    }

    public void ChangeDaytime()
    {
        _ambient_anim.SetTrigger("TransitionTime");
        _sm.enabled = true;
    }
}
