using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLive : MonoBehaviour
{
    
    [HideInInspector()] public float health;
    public Slider healthSlider;

    public SimpleSampleCharacterControl character;

    private float initCharSpeed;

    public static int alive = 1;

    public PlayerUI ui;

    public PlayerKeysController keys;

    public PlayerFX fx;

    public GameObject finishDoor;

    public PlayerAudioEffects audio;

    void Start(){

        alive = 1;
        health = 100;
        healthSlider.value = health;
        initCharSpeed = character.m_moveSpeed;
    }



    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Key"){
            fx.playKeyGetEffect();
            audio.playKeyGet();
            keys.updateKeysValue(1);
            Destroy(other.gameObject);
        }
        else if(other.gameObject.tag == "Finish"){
            finishDoor.GetComponent<Animator>().enabled=true;
            audio.playWin();
            win();

        }

        if(alive == 1 && gameObject.transform.position.y < -4){
            death();
            ui.setDeathPanelVisible(true);
            alive = -1;
            //audio.playDeath();
        }
    }

    void OnTriggerStay(Collider other){
        if(other.gameObject.tag == "Water"){
            if(character.m_moveSpeed > 4){
                Debug.Log("water");
                character.m_moveSpeed *= 0.997f;
                
            }

        Debug.Log(gameObject.transform.position.y + gameObject.transform.localScale.y/2.5f);
            if(other.gameObject.transform.parent.gameObject.transform.position.y > (gameObject.transform.position.y + gameObject.transform.localScale.y/2.5f)){
                if(alive == 1)updateHealth(0.1f);
            }
        }

        if(alive == 1 && gameObject.transform.position.y < -4){
            death();
            ui.setDeathPanelVisible(true);
            alive = -1;
            //audio.playDeath();
        }
    }

    void OnTriggerExit(Collider other){
        if(other.gameObject.tag == "Water"){
            character.m_moveSpeed = initCharSpeed;
        }
    }


    public void updateHealth(float diff){
        if(health > 0){
            health -= diff;
            healthSlider.value = health;
        }
        else{
            health = 0;
        }

        if(health == 0){
            death();
            ui.setDeathPanelVisible(true);
            alive = 0;
            audio.playDeath();
        }
    }

    public void win(){
        ui.setWinPanelVisible(true);
        alive = 2;
        PlayerPrefs.SetInt("currentLevel", Application.loadedLevel-1);
    }


    public void death(){
 
        Destroy(character.m_animator);
        Destroy(gameObject.GetComponent<SimpleSampleCharacterControl>());

        transform.position=new Vector3(transform.position.x, -1.41f, transform.position.z);
        StartCoroutine(deathClip());
        Invoke(nameof(deathBody), 16f);
        //Destroy(gameObject.GetComponent<HumanCollision>());
    }


    IEnumerator deathClip(){
        //Destroy(rigidbody);
        while(transform.eulerAngles.x != -82.5f){
            transform.eulerAngles = Vector3.MoveTowards(transform.eulerAngles, new Vector3(-82.5f,transform.eulerAngles.y, transform.eulerAngles.z ), 0.6f);
            
            yield return new WaitForEndOfFrame();
        }
        
        
    }

    void deathBody(){
        var colliders=gameObject.GetComponents<BoxCollider>();
        foreach(var collider in colliders)collider.isTrigger=true;
        gameObject.AddComponent<Rigidbody>().useGravity=true;
        Invoke(nameof(clean),4f);
    }

    void clean(){
        Destroy(gameObject);
    }

    
}
