using System;
using UnityEngine;
using UnityEngine.UI;

namespace SheildMaster {
    public class GameEventManager : MonoBehaviour {

        [SerializeField] private int currentDay;
        [SerializeField] private PlayerDataSO playerData;
        [SerializeField] private DailyBonuUIButton bonusButton;
        [SerializeField] private GameObject eventCanvas;
        [SerializeField] private DayOfWeek dayOfTheWeek;
        [SerializeField] private DailyRewardSO[] dailyRewardSOs;
        private void Awake(){
            dayOfTheWeek = DayOfWeek.Saturday;
        }
        private void Start() {
            int currentDay = (int)dayOfTheWeek;
            if(playerData.GetcurrentDay() != currentDay){
                playerData.SetDailyBonusAlreadyShown(false);
                playerData.SetcurrentDay(currentDay);
            }else{
                playerData.SetDailyBonusAlreadyShown(true);
            }
            for (int i = 0; i < dailyRewardSOs.Length; i++){
                if(currentDay == i){
                    bonusButton.setTodayReward(dailyRewardSOs[currentDay]);
                    break;
                }
            }
            bonusButton.SetTodayRewardView();
            UpdateCurrentDay();
            
        }
        
        private void UpdateCurrentDay(){
            if(!playerData.GetCurentDailyBonuShown()){
                eventCanvas.SetActive(true);
            }else{
                eventCanvas.SetActive(false);
            }
        }
        
        
        
    }

}