using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
namespace SheildMaster {
    public class AskAdForCoin : MonoBehaviour {
        [SerializeField] private CoinMultiplier coinMultiplier;
        [SerializeField] private PlayerDataSO playerData;
        [SerializeField] private Button watchAdButton;
        [SerializeField] private IAPItemSO itemSO;
        [SerializeField] private TextMeshProUGUI totalCoinAmount;
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private GameObject overlay;
        [SerializeField] private TimeManager timeManager;
        private AdController adController;
        public static AskAdForCoin askAdForCoinCurrent;
        private void Awake(){
            if(askAdForCoinCurrent == null){
                askAdForCoinCurrent = this;
            }
        }
        private void Start(){
            adController = AdController.current;
            adController.askingforExtraCoinFromShop = true;
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
            AdController.current.ShowRewarededAds();
        }
        public void RewardCoinWithCoins(bool canReward){
            if(canReward){
                coinMultiplier.CollectCoin(itemSO.CoinAmount);
                AudioManager.current.PlayOneShotMusic(SoundType.Item_Purchase);
                timeManager.Click();
            }
        }
        private void RefreshCoinAmount(){
            totalCoinAmount.SetText(playerData.GetTotalCoinValue().ToString());
        }
        
    }
}
