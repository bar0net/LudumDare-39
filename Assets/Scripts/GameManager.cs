﻿using System.Collections;
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

    public float taxRate = 0.02f;
    public float powerConsume = 0.001f;

    int powerGeneration = 0;
    bool isNight = false;

    // Before Anything Else
    private void Awake()
    {
        towerToBuild = null;
        currPower = maxPower;
    }

    // Use this for initialization
    void Start () {

        _sm = FindObjectOfType<SpawnManager>();
        _sm.enabled = false;

        _ambient_anim = GameObject.Find("Ambient").GetComponent<Animator>();

        _ui = FindObjectOfType<UIManager>();
        _ui.UpdatePower(currPower, maxPower, 0);
        _ui.UpdatePopulation(population);
        _ui.UpdateMoney(money);
        _ui.ChangeTaxText("-" + Mathf.CeilToInt(powerConsume * population).ToString(), 0);

        Turret[] all_t = FindObjectsOfType<Turret>();
        foreach (Turret t in all_t)
        {
            if (t.type == Turret.DamageTypes.SOLAR) powerGeneration += (Mathf.FloorToInt(2 * t.damage / 3));
        }
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
        isNight = toNight;

        if (toNight)
        {
            _ui.ChangeTaxText("$+" + Mathf.CeilToInt(taxRate * population).ToString(), 1);
            DrainPower(Mathf.CeilToInt(powerConsume * population));
        }
        else
        {
            DrainPower(-powerGeneration, false);

            _ui.ChangeTaxText("-" + Mathf.CeilToInt(powerConsume * population).ToString(), 0);
            EarnMoney(Mathf.CeilToInt(taxRate * population));
        }
    }

    public void DrainPower(int value, bool showGeneration = true)
    {
        currPower -= value;

        if (currPower > maxPower) currPower = maxPower;
        else if (currPower < 0) currPower = 0;

        _ui.UpdatePower(currPower, maxPower, powerGeneration);
    }

    public void ExpandMaxPower(int value)
    {
        powerGeneration += Mathf.FloorToInt(2 * value / 3);
        maxPower += value;
        
        if (isNight) _ui.UpdatePower(currPower, maxPower, powerGeneration);
        else _ui.UpdatePower(currPower, maxPower, 0);
    }

    public void KillPopulation(int value)
    {
        population -= value;

        if (population < 0) population = 0;
        _ui.UpdatePopulation(population);
        _ui.ChangeTaxText("$" + Mathf.CeilToInt(taxRate * population).ToString());

        if (population == 0) GameOver();
    }

    public void EarnMoney(int value)
    {
        money += value;

        if (money < 0) money = 0;

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
