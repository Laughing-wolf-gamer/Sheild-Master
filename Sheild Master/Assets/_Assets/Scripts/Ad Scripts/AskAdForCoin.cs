using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SheildMaster {
    public class AskAdForCoin : MonoBehaviour {
        [SerializeField] private PlayerDataSO playerData;
        [SerializeField] private Button watchAdButton;
        [SerializeField] private IAPItemSO itemSO;
        [SerializeField] private TextMeshProUGUI totalCoinAmount;
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
            playerData.onCurrencyAmountChange += RefreshCoinAmount;
        }
        private void Update(){
            checkAdStatus();
        }
        private void checkAdStatus(){
            if(adController != null){
                if(adController.IsRewardedAdsLoaded()){
                    watchAdButton.interactable = true;
                }else{
                    watchAdButton.interactable = false;
                }
            }
        }
        public void RequestAdForCoin(){
            AdController.current.ShowRewarededAds();
        }
        public void RewardCoinWithCoins(bool canReward){
            if(canReward){
                playerData.AddCoins(itemSO.CoinAmount);
                AudioManager.current.PlayOneShotMusic(SoundType.Item_Purchase);
            }
        }
        private void RefreshCoinAmount(){
            totalCoinAmount.SetText(playerData.GetTotalCoinValue().ToString());
        }
        
    }
}
