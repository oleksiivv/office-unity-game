using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterLift : MonoBehaviour
{
    public float speed;

    public int targetyPos = 32;

    void Update(){
        //transform.Translate(Vector3.up/10*speed);
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, targetyPos, transform.position.z), 0.1f*speed * PlayerLive.alive*Time.timeScale);
    }
}
