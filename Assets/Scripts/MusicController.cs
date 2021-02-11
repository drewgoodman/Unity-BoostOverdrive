using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    // use MusicController tag and call PlayMusic from camera script

    private AudioSource musicSource;

    // TODO: Handle duplicate music objects while preserving continuous audio between scenes

    void Start()
    {
        DontDestroyOnLoad(transform.gameObject);
        musicSource = GetComponent<AudioSource>();
    }

    public void PlayMusic()
    {
        if (musicSource.isPlaying) { return; }
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }
}
