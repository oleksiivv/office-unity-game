using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AdmobController))]
[RequireComponent(typeof(UnityAdsController))]
public class PlayerUI : MonoBehaviour
{
    public GameObject winPanel, deathPanel, pausePanel;

    public ScenesManager scenes;

    public static int addCnt = 1;

    private AdmobController admobController;

    void Start(){
        admobController = GetComponent<AdmobController>();
        admobController.unityAds = GetComponent<UnityAdsController>();

        admobController.Init();
    }

    public void pause(){
        pausePanel.SetActive(true);
        Time.timeScale = 0;      

        admobController.showIntersitionalAd();
    }

    public void resume(){
        pausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void openScene(int id){
        Time.timeScale = 1;
        scenes.openScene(id);
    }

    public void restart(){
        openScene(Application.loadedLevel);
    }

    public void next(){
        openScene(Application.loadedLevel+1);
    }

    public void setDeathPanelVisible(bool visible){
        deathPanel.SetActive(visible);

        if(visible){
            admobController.showIntersitionalAd();
        }
    }

    public void setWinPanelVisible(bool visible){
        winPanel.SetActive(visible);
    }
}
