using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public const float _BULLET_PROXIMITY_ = 0.2f;

    public GameObject[] towerTypes;

    public GameObject towerToBuild { private set; get; }

    public int currPower = 100;
    public int maxPower = 100;

    public int population = 9999;

    public int money = 0;
    
    SpawnManager _sm;
    Animator _ambient_anim;
    UIManager _ui;

    public bool toogleingPower = false;

    private void Awake()
    {
        towerToBuild = null;

        currPower = 100;
        maxPower = 100;
    }

    // Use this for initialization
    void Start () {

        _sm = FindObjectOfType<SpawnManager>();
        _sm.enabled = false;

        _ambient_anim = GameObject.Find("Ambient").GetComponent<Animator>();

        _ui = FindObjectOfType<UIManager>();
        _ui.UpdatePower(currPower, maxPower);
        _ui.UpdatePopulation(population);
        _ui.UpdateMoney(money);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.KeypadEnter)) ChangeDaytime(true);
        if (Input.GetMouseButtonUp(1)) {
            if (toogleingPower) ChangeTooglePower();
            _ui.UnHighlightTurretBuilder();
        }
	}

    public void ChangeTimeScale(float scale)
    {
        Time.timeScale = scale;
    }

    public void SetTowerToBuild(int index)
    {
        if (toogleingPower) ChangeTooglePower();

        if (index == -1)
        {
            towerToBuild = null;
            _ui.UnHighlightTurretBuilder();
        }
        else if (index != -1 && index < towerTypes.Length)
        {
            towerToBuild = towerTypes[index];
            _ui.HighlightTurretBuilder(index);
        }
    }

    public void ChangeDaytime(bool toNight)
    {
        _ambient_anim.SetTrigger("TransitionTime");
        _sm.enabled = toNight;
        _ui.EnableTimeButton(!toNight);
    }

    public void DrainPower(int value)
    {
        currPower -= value;
        _ui.UpdatePower(currPower, maxPower);
    }

    public void ExpandMaxPower(int value)
    {
        maxPower += value;
        _ui.UpdatePower(currPower, maxPower);
    }

    public void KillPopulation(int value)
    {
        population -= value;

        if (population < 0) population = 0;
        _ui.UpdatePopulation(population);

        if (population == 0) GameOver();
    }

    public void EarnMoney(int value)
    {
        money += value;
        _ui.UpdateMoney(money);
    }

    void GameOver()
    {
        Debug.Log("Everyone died!");
    }

    public void ChangeTooglePower()
    {
        toogleingPower = !toogleingPower;
        _ui.HighlightTooglePower(toogleingPower);
    }
}
