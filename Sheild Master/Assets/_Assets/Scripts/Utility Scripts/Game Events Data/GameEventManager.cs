using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace SheildMaster {
    public class GameEventManager : MonoBehaviour {

        [SerializeField] private int currentDay;
        [SerializeField] private TextMeshProUGUI[] totalCoinTexts;
        [SerializeField] private PlayerDataSO playerData;
        [SerializeField] private Button claimRewardButton;
        [SerializeField] private GameObject eventCanvas;
        [SerializeField] private DayOfWeek dayOfTheWeek;
        [SerializeField] private DailyBonuUIButton[] rewardClameButtonArray;
        [SerializeField] private DailyBonuUIButton CurrentBonusButton;

        private void Awake(){
            dayOfTheWeek = DateTime.Today.DayOfWeek;
        }
        private void Start() {
            
            int currentDay = (int)dayOfTheWeek;
            if(playerData.GetcurrentDay() != currentDay){
                playerData.SetClamedBonus(false);
                playerData.SetcurrentDay(currentDay);
                if(!playerData.GetDailyBonusWindowAlreadyShown()){
                    if(playerData.GetIsClamedBonus()){
                        
                        eventCanvas.SetActive(false);
                        playerData.SetDailyBonusAlreadyShown(false);
                    }else{
                        eventCanvas.SetActive(true);
                        // playerData.SetDailyBonusAlreadyShown(true);
                    }
                }else{
                    eventCanvas.SetActive(false);
                    playerData.SetDailyBonusAlreadyShown(false);
                }
            }
            for (int i = 0; i < rewardClameButtonArray.Length; i++){
                if(!playerData.GetIsClamedBonus()){
                    if(i == currentDay){
                        rewardClameButtonArray[i].SetIsActive(true);
                        CurrentBonusButton = rewardClameButtonArray[i];
                    }else{
                        rewardClameButtonArray[i].SetIsActive(false);
                    }

                }else{
                    rewardClameButtonArray[i].SetIsActive(false);
                }
            }
            if(!playerData.GetIsClamedBonus()){
                claimRewardButton.interactable = true;
            }else{
                claimRewardButton.interactable = false;
            }
            RefershCoin();

        }
        private void RefershCoin(){
            for (int i = 0; i < totalCoinTexts.Length; i++){
                totalCoinTexts[i].SetText(playerData.GetCoinValue().ToString()) ;
            }
        }
        
        public void SetAlreadShownEventCanvas(){
            playerData.SetDailyBonusAlreadyShown(true);
        }
        public void Clame(){
            
            CurrentBonusButton.ClamReward();
            if(!playerData.GetIsClamedBonus()){
                claimRewardButton.interactable = true;
            }else{
                claimRewardButton.interactable = false;
            }
            CurrentBonusButton.SetIsActive(false);
            RefershCoin();
        }
        
        private void OnApplicationQuit(){
            if(!playerData.GetIsClamedBonus()){
                playerData.SetcurrentDay(-1);
            }
        }
        
        
        
        
        
    }

}