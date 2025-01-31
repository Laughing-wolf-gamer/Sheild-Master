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
        [SerializeField] private Button claimRewardButton,claimDoubleButton;
        [SerializeField] private GameObject eventCanvas;
        [SerializeField] private UIBackSpaceOnOff uIBackSpaceOnOff;
        [SerializeField] private DailyBonuUIButton[] rewardClameButtonArray;
        [SerializeField] private TimeManager timeManager;
        private DailyBonuUIButton CurrentBonusButton;
        private AdManager adManager;

        #region Singelton.......

        public static GameEventManager eventManager{get;private set;}
        private void Awake(){
            if(eventManager == null){
                eventManager = this;
            }else{
                Destroy(eventManager);
            }
        }

        #endregion

        private void Start() {
            adManager = AdManager.instance;
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
                CheckRewardButton();
                yield return null;
            }
        }
        

        private void CheckRewardButton(){
            currentDay = playerData.GetcurrentDay();

            if(!playerData.GetIsClaimed5X()){
                if(adManager.IsRewardedAdsLoaded){
                    claimDoubleButton.interactable = true;
                }else{
                    claimDoubleButton.interactable = false;
                }
            }else{
                claimDoubleButton.interactable = false;
            }
                // claimDoubleButton.interactable = false;
            if(timeManager.Ready()){
                if(playerData.GetIsClaimed5X()){
                    playerData.SetIsClaimed5XBonus(false);
                }
                playerData.SetClamedBonus(false);
            }else{
                playerData.SetClamedBonus(true);
            }
            for (int i = 0; i < rewardClameButtonArray.Length; i++){
                if(timeManager.Ready()) {

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
            CurrentBonusButton.ClamReward();
            RefershCoin();
            RefreshDimond();
            timeManager.Click();
            CheckRewardButton();
            CheckForDailyReward();
            playerData.SetClamedBonus(true);
            playerData.IncreaseCurrentDayNumber();
            CurrentBonusButton.SetIsActive(false);
            claimRewardButton.interactable = false;
        }

        //Change this According to 5X button & call it in AdManager Script.
        public void Claim5X(){
            CurrentBonusButton.ClamReward5X();
            RefershCoin();
            RefreshDimond();
            // timeManager.Click();
            CheckRewardButton();
            CheckForDailyReward();
            //Change Below function
            playerData.SetClamedBonus(true);
            playerData.IncreaseCurrentDayNumber();
            CurrentBonusButton.SetIsActive(false);
            claimDoubleButton.interactable = false;
            playerData.SetIsClaimed5XBonus(true);
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