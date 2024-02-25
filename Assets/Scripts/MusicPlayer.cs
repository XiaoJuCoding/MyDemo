using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioSource fristSource, sansecondSource;

    private void Awake()
    {
        fristSource.Play();
    }
    void Update()
    {
        if (PauseMenu.GameIsPaused)
        {
           fristSource.Pause();
        }
    }
}
