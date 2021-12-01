using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;
using System.Collections.Generic;
namespace SheildMaster {
    public class GameEventManager : MonoBehaviour {

        [SerializeField] private int currentDay;
        [SerializeField] private PlayerDataSO playerData;
        [SerializeField] private DailyBonuUIButton bonusButton;
        [SerializeField] private GameObject eventCanvas;
        [SerializeField] private DayOfWeek dayOfTheWeek;
        [SerializeField] private DailyRewardSO[] dailyRewardSOs;

        private void Awake(){
            dayOfTheWeek = DateTime.Today.DayOfWeek;
        }
        private void Start() {
            int currentDay = (int)dayOfTheWeek;
            if(playerData.GetcurrentDay() != currentDay){
                playerData.SetcurrentDay(currentDay);
                if(!playerData.GetDailyBonusWindowAlreadyShown()){
                    if(playerData.GetIsClamedBonus()){
                        eventCanvas.SetActive(false);
                        playerData.SetDailyBonusAlreadyShown(false);
                    }else{
                        eventCanvas.SetActive(true);
                        playerData.SetDailyBonusAlreadyShown(true);
                    }
                }else{
                    eventCanvas.SetActive(false);
                    playerData.SetDailyBonusAlreadyShown(false);
                }
            }
            for (int i = 0; i < dailyRewardSOs.Length; i++){
                if(currentDay == i){
                    bonusButton.setTodayReward(dailyRewardSOs[currentDay]);
                    break;
                }
            }
            bonusButton.SetTodayRewardView();
            
            
        }
        public void SetAlreadShownEventCanvas(){
            playerData.SetDailyBonusAlreadyShown(true);
        }
        
        
        
        #region Game Anylytics.........
        public static void GetRewardAdClicked(){
            AnalyticsResult result = Analytics.CustomEvent("Reward Ad Clilcked");
            Debug.Log("Rewared Ads Watch anyltics Result " + result);
        }
        private static int count = 0;
        public static void GetInterStetialAdData(){
            count ++;
            AnalyticsResult result = Analytics.CustomEvent("Intersteial Ad Shown",new Dictionary<string,object>{
                {"Watch Count",count},
            });
            Debug.Log("Interstetial Ads Watch anyltics Result " + result);
        }
        public static void OnGameWon(string levelName,float liveTime){
            AnalyticsResult result = Analytics.CustomEvent("Game Play Result", new Dictionary<String,object>{
                {string.Concat("Player Survive on ", levelName," For "),liveTime}
            });
            Debug.Log("On Game Won Result "+ result);
        }
        public static void OnGameLost(string levelName,float liveTime,int DeathCount){
            AnalyticsResult result = Analytics.CustomEvent("Game Play Result", new Dictionary<String,object>{
                {string.Concat("Player survived on ", levelName, " For "),liveTime},{string.Concat("Player died on ", levelName),DeathCount}
            });
            Debug.Log("On Game Loss Result "+ result);
        }
        public static void SetShopItemPurcases(string itemName){
            AnalyticsResult result = Analytics.CustomEvent(string.Concat(itemName," is Purchased"));
            Debug.Log("Shop Item Purcase Result "+ result);
        }
        public static void SetMostUsedSkin(string itemName){
            AnalyticsResult result = Analytics.CustomEvent(string.Concat(itemName, " Is Using"));
            Debug.Log("Item Using Result "+ result);
        }
        
        #endregion
        
        
    }

}