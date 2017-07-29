using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public Image[] timeButtons;

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
}
