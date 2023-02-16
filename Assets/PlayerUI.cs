using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yodo1.MAS;
//using UnityEngine.Advertisements;

public class PlayerUI : MonoBehaviour
{
    public GameObject winPanel, deathPanel, pausePanel;

    public ScenesManager scenes;

    private string gameID = "4356945";
    public static int addCnt = 1;

    //public AdmobController admob;

    void Start(){
        //Advertisement.Initialize(gameID, false);

        InitializeSdk();
        SetPrivacy(true, false, false);
        
        InitializeInterstitialAds();
    }

    private void SetPrivacy(bool gdpr, bool coppa, bool ccpa)
    {
        Yodo1U3dMas.SetGDPR(gdpr);
        Yodo1U3dMas.SetCOPPA(coppa);
        Yodo1U3dMas.SetCCPA(ccpa);
    }

    private void InitializeSdk()
    {
        Yodo1U3dMas.InitializeSdk();
    }

    private void InitializeInterstitialAds()
    {
        Yodo1U3dMasCallback.Interstitial.OnAdOpenedEvent +=    
        OnInterstitialAdOpenedEvent;
        Yodo1U3dMasCallback.Interstitial.OnAdClosedEvent +=      
        OnInterstitialAdClosedEvent;
        Yodo1U3dMasCallback.Interstitial.OnAdErrorEvent +=      
        OnInterstitialAdErorEvent;
    }

    private void OnInterstitialAdOpenedEvent()
    {
        Debug.Log("[Yodo1 Mas] Interstitial ad opened");
    }

    private void OnInterstitialAdClosedEvent()
    {
        Debug.Log("[Yodo1 Mas] Interstitial ad closed");
    }

    private void OnInterstitialAdErorEvent(Yodo1U3dAdError adError)
    {
        Debug.Log("[Yodo1 Mas] Interstitial ad error - " + adError.ToString());
    }
    public void pause(){
        pausePanel.SetActive(true);
        Time.timeScale = 0;      

        /*if(addCnt % 2 == 0){
            if(Advertisement.IsReady("Interstitial_Android")){
                Advertisement.Show("Interstitial_Android");
            }
        }

        addCnt++;*/

        if(addCnt%2 == 0){
            if(Yodo1U3dMas.IsInterstitialAdLoaded()){
                Yodo1U3dMas.ShowInterstitialAd();
            }
        }
        addCnt++;
    }

    public void resume(){
        pausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void openScene(int id){
        Time.timeScale = 1;
        scenes.openScene(id);
        //Application.LoadLevel(id);
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
            /*if(addCnt % 2 == 0){
                if(!admob.showIntersitionalAd()){
                    if(Advertisement.IsReady("Interstitial_Android")){
                        Advertisement.Show("Interstitial_Android");
                    }
                }
            }

            addCnt++;   */
            if(addCnt%2 == 0){
                if(Yodo1U3dMas.IsInterstitialAdLoaded()){
                    Yodo1U3dMas.ShowInterstitialAd();
                }
            }
            addCnt++;
        }
    }

    public void setWinPanelVisible(bool visible){
        winPanel.SetActive(visible);
    }
}
