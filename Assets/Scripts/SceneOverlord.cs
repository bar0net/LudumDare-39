using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneOverlord : MonoBehaviour {

    // Scene transitions
    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    // Quit the application
    public void Quit()
    {
        Application.Quit();
    }

    // Reload the same scene
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Delete seed from player prefabs
    // I am storing the value of the seed whenever a user loads the proceduraly
    // generated level to allow a restart on the same map.
    public void DeleteSeed()
    {
        PlayerPrefs.DeleteKey("seed");
    }
}
