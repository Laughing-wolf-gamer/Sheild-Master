using System;
using UnityEngine;
using Firebase.Analytics;
using UnityEngine.Analytics;
using System.Collections.Generic;
public class AnayltyicsManager : MonoBehaviour {
    
    public static AnayltyicsManager current;
    private void Awake(){
        if(current == null){
            current = this;
        }else {
            Destroy(current.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }
    
    #region Firebase Anaylytics.......
    private void Start(){
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventAppOpen);
    }
    private void OnApplicationQuit(){
        SetviewTime();
    }
    public void SetPlayerLevelAnaylytics(int currentLevel){
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelUp,"Level",currentLevel);
    }
    public void SetAdImpressionDataAnayltyics(){
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventAdImpression);
    }
    public void SetviewTime(){
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventScreenView);
    }
    public void SetSpendRealCurrencyForCoins(int amount){
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventPurchase,"Purchase Coin ",amount);
    }

    #endregion


    #region Unity Anylytics.........
    private  int count = 0;
    public void OnAdcompleteAnayltyics_UnityAnayltics(bool isRewardAd,AdvertisingNetwork network){
        count ++;
        AnalyticsResult result = AnalyticsEvent.AdComplete(isRewardAd,network,null,new Dictionary<string,object>{
            {"Watch Count",count},
        });
        Debug.Log(network.ToString() + result);
    }
    public void OnAdStartAnayltyics_UnityAnayltics(bool isRewardAd,AdvertisingNetwork network){
        AnalyticsResult result = AnalyticsEvent.AdStart(isRewardAd,network);
        Debug.Log(network.ToString() + result);
    }
    
    public  void OnGameWon_UnityAnayltics(string levelName,float liveTime){
        AnalyticsResult result = Analytics.CustomEvent("Game Play Result", new Dictionary<String,object>{
            {string.Concat("Player Survive on ", levelName," For "),liveTime}
        });
        Debug.Log("On Game Won Result "+ result);
    }
    public void OnGameLost_UnityAnayltics(string levelName,float liveTime,int DeathCount){
        AnalyticsResult result = Analytics.CustomEvent("Game Play Result", new Dictionary<String,object>{
            {string.Concat("Player survived on ", levelName, " For "),liveTime},{string.Concat("Player died on ", levelName),DeathCount}
        });
        Debug.Log("On Game Loss Result "+ result);
    }
    public void SetShopItemPurcases_UnityAnayltics(string itemName){
        AnalyticsResult result = Analytics.CustomEvent(string.Concat(itemName," is Purchased"));
        Debug.Log("Shop Item Purcase Result "+ result);
    }
    public void SetMostUsedSkin_UnityAnayltics(string itemName){
        AnalyticsResult result = Analytics.CustomEvent(string.Concat(itemName, " Is Using"));
        Debug.Log("Item Using Result "+ result);
    }
    
    
    #endregion
    
}
