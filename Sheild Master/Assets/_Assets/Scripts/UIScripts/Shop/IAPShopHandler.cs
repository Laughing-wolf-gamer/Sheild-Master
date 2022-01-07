using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SheildMaster {
    public class IAPShopHandler : MonoBehaviour {
        [SerializeField] private IAPItemSO itemSO;
        [SerializeField] private Button buyButton;
        [SerializeField] private TextMeshProUGUI coinAmountText;
        [SerializeField] private TextMeshProUGUI totalCoinAmountTexts;
        [SerializeField] private PlayerDataSO playerData;

        
        private void Start(){
            SetUP();
            playerData.onCurrencyValueChange += RefreshCoinAmount;
            RefreshCoinAmount();

        }
        

        private void SetUP(){
            coinAmountText.SetText(string.Concat(itemSO.CoinAmount.ToString()," CASH"));
            RefreshCoinAmount();
        }
        private void RefreshCoinAmount(){
            totalCoinAmountTexts.SetText(string.Concat(playerData.GetCashAmount().ToString()));
        }
        
        public void OnPurchaseComplete(){
            AnayltyicsManager.current.SetSpendRealCurrencyForCoins(itemSO.CoinAmount);
            if(itemSO.CoinAmount >= 200){
                PlayGamesController.PostAchivements(GPGSIds.achievement_first_spend);
            }
            if(itemSO.CoinAmount >= 2000){
                PlayGamesController.PostAchivements(GPGSIds.achievement_big_spender);
            }
            if(itemSO.CoinAmount >= 10000){
                PlayGamesController.PostAchivements(GPGSIds.achievement_super_spender);
            }
            playerData.AddCoins(itemSO.CoinAmount);
            RefreshCoinAmount();
        }
        public void OnPurchaseFailed(){
            RefreshCoinAmount();
        }
        public void OnRemoveAds_PurchaseComplete(bool value){
            playerData.SetHasAdsInGame(value);
        }
        
    }

}