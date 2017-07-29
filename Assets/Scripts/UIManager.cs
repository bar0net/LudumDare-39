using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public Image[] timeButtons;
    public Text textPower;
    public Text textPopulation;
    public Text textMoney;
    public Button timeButton;
    public Image tooglePower;
    public Image[] turretButtons;

    int turretButtonHighlighted = -1;
    private void Start()
    {
        TimeHighlight(2); // highlight play at the begining
    }

    public void TimeHighlight(int index)
    {
        for (int i = 0; i < timeButtons.Length; ++i) {

            if (i == index) timeButtons[i].color = Color.red;
            else timeButtons[i].color = Color.white;
        }
    }

    public void UpdatePower(int currPower, int maxPower)
    {
        textPower.text = currPower.ToString() + " / " + maxPower.ToString();
    }

    public void UpdatePopulation(int population)
    {
        textPopulation.text = population.ToString();
    }

    public void UpdateMoney(int money)
    {
        textMoney.text = "$" + money;
    }

    public void EnableTimeButton(bool isActive)
    {
        timeButton.interactable = isActive;
    }

    public void HighlightTooglePower(bool active)
    {
        if (turretButtonHighlighted != -1) UnHighlightTurretBuilder();

        if (active) tooglePower.color = new Color(0.5f,1.0f,1.0f);
        else tooglePower.color = Color.white;
    }

    public void HighlightTurretBuilder(int index)
    {
        if (turretButtonHighlighted != -1) UnHighlightTurretBuilder();

        turretButtons[index].color = new Color(0.5f, 1.0f, 1.0f);
        turretButtonHighlighted = index;
    }

    public void UnHighlightTurretBuilder()
    {
        if (turretButtonHighlighted == -1) return;

        turretButtons[turretButtonHighlighted].color = Color.white;
    }
}
