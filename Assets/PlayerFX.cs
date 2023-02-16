using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFX : MonoBehaviour
{
    public ParticleSystem keyGetEffect;
    public ParticleSystem coinGetEffect;

    public void playCoinGet(){
        coinGetEffect.Play();
    }

    public void playKeyGetEffect(){
        keyGetEffect.Play();
    }
}
