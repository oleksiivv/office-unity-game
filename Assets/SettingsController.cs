using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    public GameObject undoProgressPanel;

    public void undoProgress(){
        undoProgressPanel.SetActive(!undoProgressPanel.activeSelf);
    }
    public void undoProgressConfirm(){
        PlayerPrefs.DeleteAll();
        undoProgressPanel.SetActive(false);
    }

    public void undoProgressCancel(){
        undoProgressPanel.SetActive(false);
    }

    public void openScene(int id){
        scenes.openScene(id);
    }




    public GameObject buttonMutedSound, buttonNormalSound;
    public GameObject buttonMutedMusic, buttonNormalMusic;
    public AudioController audio;

    public Dropdown dropdown;

    public ScenesManager scenes;
    void Start()
    {
        dropdown.GetComponent<Dropdown>().value=PlayerPrefs.GetInt("qualityLevel",0);
        updateMusic();
        updateSound();
    }

    public void muteSound(){
        PlayerPrefs.SetInt("!sound",1);
        updateSound();

        //GetComponent<AudioSource>().enabled=false;
    }

    public void unmuteSound(){
        PlayerPrefs.SetInt("!sound",0);
        updateSound();

        //GetComponent<AudioSource>().enabled=true;
    }


    public void muteMusic(){
        PlayerPrefs.SetInt("!music",1);
        updateMusic();
    }

    public void unmuteMusic(){
        PlayerPrefs.SetInt("!music",0);
        updateMusic();
    }

    void updateSound(){
        if(PlayerPrefs.GetInt("!sound")==0){

            buttonMutedSound.SetActive(false);
            buttonNormalSound.SetActive(true);

        }
        else{
            buttonMutedSound.SetActive(true);
            buttonNormalSound.SetActive(false);
        }

        audio.updateMusic();
        audio.updateSound();
    }

    void updateMusic(){
        if(PlayerPrefs.GetInt("!music")==0){

            buttonMutedMusic.SetActive(false);
            buttonNormalMusic.SetActive(true);

        }
        else{
            buttonMutedMusic.SetActive(true);
            buttonNormalMusic.SetActive(false);
        }

        audio.updateMusic();
        audio.updateSound();
    }

    public void SetQuality(int qualityIndex){
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("qualityLevel", qualityIndex);
    }



}
