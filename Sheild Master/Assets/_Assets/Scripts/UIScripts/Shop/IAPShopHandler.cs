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
        [SerializeField] private bool isDimondShop = true;
        
        private void Start(){
            SetUP();
            if(!isDimondShop){
                playerData.onCurrencyValueChange += RefreshCoinAmount;
                RefreshCoinAmount();
            }else{
                playerData.onDimondValueChange += RefreshDimondAmount;
                RefreshDimondAmount();

            }

        }
        private void RefreshDimondAmount(){
            totalCoinAmountTexts.SetText(string.Concat(playerData.GetDimondCount().ToString()));
        }
        
        

        private void SetUP(){
            string lastText = "";
            if(!isDimondShop){
                lastText = " CASH";
                RefreshCoinAmount();
            }else{
                lastText = " DIAMOND";
                RefreshDimondAmount();
            }
            coinAmountText.SetText(string.Concat(itemSO.CoinAmount.ToString(),lastText));
        }
        private void RefreshCoinAmount(){
            totalCoinAmountTexts.SetText(string.Concat(playerData.GetCashAmount().ToString()));
        }
        
        public void OnPurchaseComplete(){
            AnayltyicsManager.current.SetSpendRealCurrencyForCoins(itemSO.CoinAmount);
            if(!isDimondShop){
                playerData.AddCoins(itemSO.CoinAmount);
            }else{
                playerData.AddDimond(itemSO.CoinAmount);
            }
            if(itemSO.CoinAmount >= 200){
                PlayGamesController.PostAchivements(GPGSIds.achievement_first_spend);
            }
            if(itemSO.CoinAmount >= 2000){
                PlayGamesController.PostAchivements(GPGSIds.achievement_big_spender);
            }
            if(itemSO.CoinAmount >= 10000){
                PlayGamesController.PostAchivements(GPGSIds.achievement_super_spender);
            }
            RefreshCoinAmount();
            RefreshDimondAmount();
        }
        public void OnPurchaseFailed(){
            Debug.Log("Purchase Failed");
            RefreshCoinAmount();
            RefreshDimondAmount();
        }
        public void OnRemoveAds_PurchaseComplete(bool value){
            playerData.SetHasAdsInGame(value);
        }
        
    }

}