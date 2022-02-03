using UnityEngine;
using UnityEngine.UI;


namespace SheildMaster {
    public class RemoveAdsShopIAP : MonoBehaviour {

        [SerializeField] private Button buyButton;
        [SerializeField] private PlayerDataSO playerData;
        private void Start(){
            if(playerData.GetHasAdsInGame()){
                buyButton.interactable = true;
            }else{
                buyButton.interactable = false;
            }
        }
        public void OnPurchaseCompleted(){
            playerData.SetHasAdsInGame(false);
            buyButton.interactable = false;
        }
        public void OnPurchaseFailed(){
            playerData.SetHasAdsInGame(true);
            buyButton.interactable = true;
        }
    }

}