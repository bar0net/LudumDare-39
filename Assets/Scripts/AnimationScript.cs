using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScript : MonoBehaviour {
    AudioSource _as;
    GameManager _gm;

    // Get the components on start
    private void Start()
    {
        _as = GetComponent<AudioSource>();
        _gm = FindObjectOfType<GameManager>();
    }

    // Plays the sound on the AudioSource component if sound is allowed
    // Used as an animation trigger
    void PlaySound()
    {
        if (_gm != null && !_gm.minionAudio) return;

        _as.pitch = Random.Range(1f, 1.5f);
        _as.Play();
    }

}
