using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    // use MusicController tag and call PlayMusic from camera script

    private AudioSource musicSource;

    void Start()
    {
        GameObject[] audioSourceRefs = GameObject.FindGameObjectsWithTag("MusicController");
        if( audioSourceRefs.Length == 1) {
            DontDestroyOnLoad(transform.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
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
