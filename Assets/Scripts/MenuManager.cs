using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {
    public GameObject vampy;
    public GameObject path;
    public float cooldown = 10f;
    float timer = 3.0f;

    public Image background;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            GameObject go = (GameObject)Instantiate(vampy, path.transform.GetChild(0).position, Quaternion.identity);
            Enemy e = go.GetComponent<Enemy>();
            e.pathing = path;

            go.GetComponentInChildren<AudioSource>().volume = 0.66f;

            timer = cooldown;
        }

        if (background != null)
            background.color = Color.Lerp(new Color(0.2f, 0, 0), new Color(0.4f, 0, 0), Mathf.Abs(Mathf.Sin(Time.time)));
	}
}
