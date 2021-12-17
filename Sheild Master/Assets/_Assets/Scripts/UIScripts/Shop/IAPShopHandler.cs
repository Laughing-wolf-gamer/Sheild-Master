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
        [SerializeField] private bool RemoveAdsWindow;
        
        private void Start(){
            
            if(!RemoveAdsWindow){
                SetUP();
            }else{
                if(playerData.GetHasAdsInGame()){
                    buyButton.interactable = true;
                }else{
                    buyButton.interactable = false;
                }
            }
            RefreshCoinAmount();
            playerData.onCurrencyAmountChange += RefreshCoinAmount;

        }
        

        private void SetUP(){
            coinAmountText.SetText(string.Concat(itemSO.CoinAmount.ToString()," CASH"));
            RefreshCoinAmount();
        }
        private void RefreshCoinAmount(){
            totalCoinAmountTexts.SetText(string.Concat(playerData.GetTotalCoinValue().ToString()));
        }
        public void Buy(){
            playerData.AddCoins(itemSO.CoinAmount);
            RefreshCoinAmount();
        }
        public void OnPurchaseComplete(){
            AnayltyicsManager.current.SetSpendRealCurrencyForCoins(itemSO.CoinAmount);
            buyButton.interactable = false;
        }
        public void OnRemoveAds_PurchaseComplete(bool value){
            playerData.SetHasAdsInGame(value);
        }
        
    }

}