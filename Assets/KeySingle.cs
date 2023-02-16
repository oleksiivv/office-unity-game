using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeySingle : MonoBehaviour
{
    void Start(){
        if(PlayerPrefs.GetInt("Key@"+gameObject.name+"@"+(Application.loadedLevel).ToString()) == 1){
            //Destroy(gameObject);
        }
    }
}
