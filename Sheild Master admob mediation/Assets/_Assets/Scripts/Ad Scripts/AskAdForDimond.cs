using TMPro;
using UnityEngine;
using UnityEngine.UI;
using GamerWolf.Utils;
using System.Collections;
namespace SheildMaster{
    public class AskAdForDimond : MonoBehaviour {
        [SerializeField] private PlayerDataSO playerData;
        [SerializeField] private Button watchAdButton;
        [SerializeField] private IAPItemSO itemSO;
        [SerializeField] private TextMeshProUGUI totalCoinAmount;
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private GameObject overlay;
        [SerializeField] private TimeManager timeManager;
        private AdManager adController;
        public static AskAdForDimond current{get;private set;}
        private void Awake(){
            if(current == null){
                current = this;
            }
        }
        private void Start(){
            checkAdStatus();
            playerData.onDimondValueChange += RefreshDimondAmount;
        }
        private void Update(){
            checkAdStatus();

        }
        
        
        private void checkAdStatus(){
            if(timeManager.Ready()){
                watchAdButton.gameObject.SetActive(true);
                overlay.SetActive(false);
                // check if the time is above the maxTime for ads.
                if(adController != null){
                    if(adController.IsRewardedAdsLoaded){
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
            AdManager.watchAdDimond = true;
            AdManager.instance.UserChoseToWatchAd();
        }
        public void RewardCoin(){
            // Reward Player with 50 coins......
            playerData.AddDimond(itemSO.CoinAmount);
            AudioManager.current.PlayOneShotMusic(SoundType.Item_Purchase);
            Invoke(nameof(InvokeTimer),0.2f);
        }
        private void InvokeTimer(){

            timeManager.Click();
        }

        private void RefreshDimondAmount(){
            totalCoinAmount.SetText(playerData.GetDimondCount().ToString());
        }
    }
}
