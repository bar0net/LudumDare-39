using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public const float _BULLET_PROXIMITY_ = 0.2f;

    public GameObject[] towerTypes;

    public GameObject towerToBuild { private set; get; }

	// Use this for initialization
	void Start () {
        towerToBuild = null;
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
}
