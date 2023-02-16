using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerKeysController : MonoBehaviour
{
    public Text keysCountText;
    private int keysCount;

    public PlayerLive player;
    public GameObject finish;

    void Start(){
        finish.SetActive(false);
        keysCount = 0;
        keysCountText.text = keysCount.ToString()+"/3";
    }


    public void updateKeysValue(int diff){
        if(keysCount < 3){
            keysCount += diff;
        }

        keysCountText.text = keysCount.ToString()+"/3";

        if(keysCount >= 3){
            
            finish.SetActive(true);

        }
    }
}
