using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SheildMaster {
    public class IAPShopHandler : MonoBehaviour {
        [SerializeField] private IAPItemSO itemSO;
        [SerializeField] private Button buyButton;
        [SerializeField] private TextMeshProUGUI coinAmountText;
        [SerializeField] private PlayerDataSO playerData;

        private void Start(){
            SetUP();
        }

        private void SetUP(){
            coinAmountText.SetText(string.Concat(itemSO.CoinAmount.ToString()));
        }
        public void Buy(){
            playerData.AddCoins(itemSO.CoinAmount);
        }
    }

}