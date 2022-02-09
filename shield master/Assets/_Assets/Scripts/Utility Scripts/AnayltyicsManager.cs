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
    public void SetTutoraialDataAnalytics(){
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventTutorialComplete);
    }
    public void SetviewTime(){
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventScreenView);
    }
    public void SetSpendRealCurrencyForCoins(int amount){
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventPurchase,"purchase_cash ",amount);
    }
    public void SetEarnVirtualCurrencyByAds(int amount,string currencyName){
        FirebaseAnalytics.LogEvent(
        FirebaseAnalytics.EventEarnVirtualCurrency,
            new Firebase.Analytics.Parameter[] {
                new Firebase.Analytics.Parameter(
                Firebase.Analytics.FirebaseAnalytics.ParameterValue, amount),
                new Firebase.Analytics.Parameter(
                Firebase.Analytics.FirebaseAnalytics.ParameterVirtualCurrencyName, currencyName),
            }
        );
    }
    public void PostScoreAnylytics(int Level){
        FirebaseAnalytics.LogEvent(
        Firebase.Analytics.FirebaseAnalytics.EventPostScore,
            new Firebase.Analytics.Parameter[] {
                // new Firebase.Analytics.Parameter(
                // Firebase.Analytics.FirebaseAnalytics.ParameterCharacter, character),
                new Firebase.Analytics.Parameter(
                Firebase.Analytics.FirebaseAnalytics.ParameterLevel, Level),
                new Firebase.Analytics.Parameter(
                Firebase.Analytics.FirebaseAnalytics.ParameterScore, Level),
            }
        );
    }
    public void PostUnLoackAcivement(string achievementId){
        FirebaseAnalytics.LogEvent(
        FirebaseAnalytics.EventUnlockAchievement,
            new Firebase.Analytics.Parameter[] {
                new Firebase.Analytics.Parameter(
                Firebase.Analytics.FirebaseAnalytics.ParameterAchievementId, achievementId),
            }
        );
    }
    public void LogInToPlayGames(){
        FirebaseAnalytics.LogEvent(
        FirebaseAnalytics.EventSignUp,
            new Firebase.Analytics.Parameter[] {
                new Firebase.Analytics.Parameter(
                Firebase.Analytics.FirebaseAnalytics.ParameterMethod,"playgames_google"),
            }
        );
    }
    public void SpendVirualCurrency(int amount,string itemName,string currencyName){
        FirebaseAnalytics.LogEvent(
        FirebaseAnalytics.EventSpendVirtualCurrency,
            new Firebase.Analytics.Parameter[] {
                new Firebase.Analytics.Parameter(
                Firebase.Analytics.FirebaseAnalytics.ParameterItemName, itemName),
                new Firebase.Analytics.Parameter(
                Firebase.Analytics.FirebaseAnalytics.ParameterValue, amount),
                new Firebase.Analytics.Parameter(
                Firebase.Analytics.FirebaseAnalytics.ParameterVirtualCurrencyName, currencyName),
            }
        );
    }
    public void SetIntersteailAdsData(){
        FirebaseAnalytics.LogEvent("interstatial_Ads");
    }
    public void SetRewardAdsData(){
        FirebaseAnalytics.LogEvent("Rewarded_ads");
        
    }
    public void SetLevelUpResult(int levelNum){
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelUp,FirebaseAnalytics.ParameterLevel,levelNum);
    }
    public void BannerAds(){
        FirebaseAnalytics.LogEvent("banner_ads");
    }

    public void ImpressionSuccesessEvent(IronSourceImpressionData impressionData){
        Debug.Log("Ironsource Impression Log Data: " + impressionData);
        if(impressionData != null){
            Firebase.Analytics.Parameter[] AdParameter = {
                new Parameter("ad_platform","ironSource"),
                new Parameter("ad_source",impressionData.adNetwork),
                new Parameter("ad_unit_name",impressionData.adUnit),
                new Parameter("ad_adformat",impressionData.instanceName),
                new Parameter("currency","USD"),
                new Parameter("value",impressionData.revenue.ToString()),
                new Parameter("life_time_value",impressionData.lifetimeRevenue.ToString())

                
            };
            FirebaseAnalytics.LogEvent("ad_impressions",AdParameter);
            

        }
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
