using TMPro;
using UnityEngine;
using UnityEngine.UI;
using GamerWolf.Utils;

namespace SheildMaster {
    public class AskAdForCoin : MonoBehaviour {
        [SerializeField] private PlayerDataSO playerData;
        [SerializeField] private Button watchAdButton;
        [SerializeField] private IAPItemSO itemSO;
        [SerializeField] private TextMeshProUGUI totalCoinAmount;
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private GameObject overlay;
        [SerializeField] private TimeManager timeManager;
        private AdManager adManager;
        public static AskAdForCoin current{get;private set;}
        private void Awake(){
            if(current == null){
                current = this;
            }
        }
        private void Start(){
            // adController = AdController.current;
            // adController.askingforExtraCoinFromShop = true;
            // adController.trySkinAds = false;
            checkAdStatus();
            playerData.onCurrencyValueChange += RefreshCoinAmount;
        }
        private void Update(){
            checkAdStatus();
        }
        
        
        
        private void checkAdStatus(){
            if(timeManager.Ready()){
                watchAdButton.gameObject.SetActive(true);
                overlay.SetActive(false);
                // check if the time is above the maxTime for ads.
                if(adManager != null){
                    
                  if(adManager.IsRewardedAdsLoaded){
                        watchAdButton.interactable = true;
                    }else{
                        watchAdButton.interactable = false;
                    }
                }
            }else{
                watchAdButton.interactable = false;
                watchAdButton.gameObject.SetActive(false);
                overlay.SetActive(true);
            }
        }
        
        public void RequestAdForCoin(){
            RequestAds();
            watchAdButton.interactable = false;
            watchAdButton.gameObject.SetActive(false);
            overlay.SetActive(true);
        }
        private void RequestAds(){
            AdManager.watchAdCoin = true;
            AdManager.instance.UserChoseToWatchAd();
        }
        public void RewardCoin(){
            // Reward Player with 50 coins......
            playerData.AddCoins(itemSO.CoinAmount);
            AudioManager.current.PlayOneShotMusic(SoundType.Item_Purchase);
            AnayltyicsManager.current.SetEarnVirtualCurrencyByAds(itemSO.CoinAmount,"Cash");
            Invoke(nameof(InvokeTimer),0.2f);
        }
        private void InvokeTimer(){

            timeManager.Click();
        }

        private void RefreshCoinAmount(){
            totalCoinAmount.SetText(playerData.GetCashAmount().ToString());
        }
        
    }
}
