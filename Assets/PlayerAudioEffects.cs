using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioEffects : MonoBehaviour
{
    public AudioSource src;
    public AudioClip keyGet, coinGet, death, win;



    public void playKeyGet(){
        src.enabled = false;
        src.clip = keyGet;
        src.enabled = PlayerPrefs.GetInt("!sound") == 0;
        src.Play();
    }

    public void playCoinGet(){
        src.enabled = false;
        src.clip = coinGet;
        src.enabled = PlayerPrefs.GetInt("!sound") == 0;
        src.Play();
    }

    public void playDeath(){
        src.enabled = false;
        src.clip = death;
        src.enabled = PlayerPrefs.GetInt("!sound") == 0;
        src.Play();
    }

    public void playWin(){
        src.enabled = false;
        src.clip = win;
        src.enabled = PlayerPrefs.GetInt("!sound") == 0;
        src.Play();
    }
}
