using TMPro;
using UnityEngine;
using UnityEngine.UI;
using GamerWolf.Utils;
using System.Collections;
namespace SheildMaster {
    public class GameEventManager : MonoBehaviour {
        [SerializeField] private int currentDay;
        [SerializeField] private float closeTime = 2f;
        [SerializeField] private GameObject mainMenuWindow;
        [SerializeField] private GameObject noitificationIcon;
        [SerializeField] private TextMeshProUGUI totalCoinTexts,totalDimondText;
        [SerializeField] private PlayerDataSO playerData;
        [SerializeField] private Button claimRewardButton;
        [SerializeField] private GameObject eventCanvas;
        [SerializeField] private UIBackSpaceOnOff uIBackSpaceOnOff;
        [SerializeField] private DailyBonuUIButton[] rewardClameButtonArray;
        [SerializeField] private TimeManager timeManager;
        private DailyBonuUIButton CurrentBonusButton;
        private void Start() {
            currentDay = playerData.GetcurrentDay();
            if(timeManager.Ready()){
                claimRewardButton.interactable = true;
                if(playerData.GetIsClamedBonus()){
                    mainMenuWindow.SetActive(true);
                    eventCanvas.SetActive(false);
                }else{
                    mainMenuWindow.SetActive(false);
                    eventCanvas.SetActive(true);
                }
            }else{
                eventCanvas.SetActive(false);
            }
            CheckRewardButton();
            RefershCoin();
            RefreshDimond();
            CheckForDailyReward();
            playerData.onCurrencyValueChange += RefershCoin;
            playerData.onDimondValueChange += RefreshDimond;
            StartCoroutine(nameof(CheckRoutine));
        }
        private IEnumerator CheckRoutine(){
            while(true){
                currentDay = playerData.GetcurrentDay();
                CheckRewardButton();
                yield return null;
            }
        }
        

        private void CheckRewardButton(){
            if(timeManager.Ready()){
                playerData.SetClamedBonus(false);
            }else{
                playerData.SetClamedBonus(true);
            }
            for (int i = 0; i < rewardClameButtonArray.Length; i++){
                if(timeManager.Ready()){

                    if(currentDay == rewardClameButtonArray[i].GetCurrentRewardNumber()){
                        rewardClameButtonArray[i].SetIsActive(true);
                        CurrentBonusButton = rewardClameButtonArray[i];
                    }else{
                        if(i < currentDay){
                            rewardClameButtonArray[i].SetClamedText("Claimed");
                        }else {
                            rewardClameButtonArray[i].SetClamedText("Come Back Later");
                        }
                        rewardClameButtonArray[i].SetIsActive(false);
                    }
                }else{
                    if(i < currentDay){
                        rewardClameButtonArray[i].SetClamedText("Claimed");
                    }else {
                        rewardClameButtonArray[i].SetClamedText("Come Back Later");
                    }
                    rewardClameButtonArray[i].SetIsActive(false);
                }
            }
            CheckForDailyReward();
        }
        private void CheckForDailyReward(){
            if(playerData.GetIsClamedBonus()){
                noitificationIcon.SetActive(false);
            }else{
                noitificationIcon.SetActive(true);
            }
        }
        
        private void RefershCoin(){
            totalCoinTexts.SetText(playerData.GetCashAmount().ToString());
        }
        private void RefreshDimond(){
            totalDimondText.SetText(playerData.GetDimondCount().ToString());
        }
        
        
        public void Claime(){
            RefershCoin();
            RefreshDimond();
            timeManager.Click();
            CheckRewardButton();
            CheckForDailyReward();
            CurrentBonusButton.ClamReward();
            playerData.SetClamedBonus(true);
            playerData.IncreaseCurrentDayNumber();
            CurrentBonusButton.SetIsActive(false);
            claimRewardButton.interactable = false;
        }
        
        
        
        public void CloseWindow(){
            RefershCoin();
            RefreshDimond();
            CancelInvoke(nameof(CloseInvoke));
            Invoke(nameof(CloseInvoke),closeTime);
        }
        private void CloseInvoke(){
            RefershCoin();
            RefreshDimond();
            mainMenuWindow.SetActive(true);
            eventCanvas.SetActive(false);
            uIBackSpaceOnOff.InvokeEscape();
        }
        
        
        
        
        
    }

}