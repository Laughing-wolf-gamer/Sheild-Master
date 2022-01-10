using TMPro;
using UnityEngine;
using UnityEngine.UI;
using GamerWolf.Utils;
using System.Collections;
namespace SheildMaster {
    public class AskAdForCoin : MonoBehaviour {
        [SerializeField] private PlayerDataSO playerData;
        [SerializeField] private Button watchAdButton;
        [SerializeField] private IAPItemSO itemSO;
        [SerializeField] private TextMeshProUGUI totalCoinAmount;
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private GameObject overlay;
        [SerializeField] private TimeManager timeManager;
        private AdController adController;
        public static AskAdForCoin current{get;private set;}
        private void Awake(){
            if(current == null){
                current = this;
            }
        }
        private void Start(){
            adController = AdController.current;
            adController.askingforExtraCoinFromShop = true;
            adController.trySkinAds = false;
            checkAdStatus();
            playerData.onCurrencyValueChange += RefreshCoinAmount;        
            StartCoroutine(CheckRoutine());
        }
        private IEnumerator CheckRoutine(){
            while(true){
                checkAdStatus();
                yield return null;
            }
        }
        
        
        private void checkAdStatus(){
            if(timeManager.Ready()){
                watchAdButton.gameObject.SetActive(true);
                overlay.SetActive(false);
                // check if the time is above the maxTime for ads.
                if(adController != null){
                    
                    if(adController.IsRewardedAdsLoaded()){
                        watchAdButton.interactable = true;
                    }else{
                        watchAdButton.interactable = false;
                        adController.SetRewardAdsCallBack();
                    }
                }
            }else{
                watchAdButton.interactable = false;
                watchAdButton.gameObject.SetActive(false);
                overlay.SetActive(true);
            }
        }
        
        public void RequestAdForCoin(){
            // Request Extra coins in the shop.
            adController.trySkinAds = false;
            adController.askingforExtraCoinFromShop =true;
            timeManager.Click();
            // AdController.current.ShowRewarededAds();
        }
        public void RewardCoinWithCoins(){

            // Reward Player with 50 coins......
            playerData.AddCoins(itemSO.CoinAmount);
            AudioManager.current.PlayOneShotMusic(SoundType.Item_Purchase);
        }
        public void OnCoinShopOpen(){
            if(adController != null){
                adController.trySkinAds = false;
                adController.askingforExtraCoinFromShop = true;
            }
        }
        public void OncoinShopClose(){
            if(adController != null){
                adController.askingforExtraCoinFromShop = false;
                adController.trySkinAds = false;
            }
        }
        private void RefreshCoinAmount(){
            totalCoinAmount.SetText(playerData.GetCashAmount().ToString());
        }
        
    }
}
