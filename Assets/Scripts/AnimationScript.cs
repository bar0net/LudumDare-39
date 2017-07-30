using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScript : MonoBehaviour {
    AudioSource _as;
    GameManager _gm;

    private void Start()
    {
        _as = GetComponent<AudioSource>();
        _gm = FindObjectOfType<GameManager>();
    }

    void PlaySound()
    {
        if (_gm != null && !_gm.minionAudio) return;

        _as.pitch = Random.Range(1f, 1.5f);
        _as.Play();
    }

}
