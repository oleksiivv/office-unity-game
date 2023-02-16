using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject levelsPanel, studyPanel;

    public ScenesManager scenes;


    void Start(){
        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("qualityLevel",0));
    }

    public void openScene(int id){
        scenes.openScene(id);
    }

    public void setLevelsPanelActive(bool active){
        
        if(PlayerPrefs.GetInt("studied",-1)==-1){
            studyPanel.SetActive(true);
            PlayerPrefs.SetInt("studied",1);
        }else{
            studyPanel.SetActive(false);
            levelsPanel.SetActive(active);
        }
    }

    public void rate(){
        Application.OpenURL("https://play.google.com/store/apps/developer?id=Vertex+Studio+Games");
    }

    public void setSecondStudyPanelActive(bool active){
        studyPanel.SetActive(active);
    }
}
