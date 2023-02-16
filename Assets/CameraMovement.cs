using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset;

    void Start(){
        offset = transform.position - player.transform.position;
    }

    void Update(){
        if(PlayerLive.alive == 1)transform.position = new Vector3(offset.x + player.transform.position.x, offset.y + player.transform.position.y, offset.z + player.transform.position.z);
    }
}
