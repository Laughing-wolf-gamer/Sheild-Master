using TMPro;
using UnityEngine;
using GamerWolf.Utils;
namespace SheildMaster {
    public class ShopHandler : MonoBehaviour {

        [SerializeField] private PlayerDataSO playerDataSO;

        [SerializeField] private TextMeshProUGUI skinNameText;
        [SerializeField] private SkinnedMeshRenderer displayPlayerSkinMat,dispalyPlayerClothMat,displayPlayerBelt;
        [SerializeField] private TextMeshProUGUI coinAmountText;
        [SerializeField] private ShopItemSO[] itemSO;
        [SerializeField] private int currentItemIndex;
        private void Start(){
            if(PlayerPrefs.HasKey("Current Index")){
                currentItemIndex = PlayerPrefs.GetInt("Current Index");
            }
            ChangeView();
        }
        public void ChangeView(){
            for(int i = 0; i < itemSO.Length; i++){
                if(i == currentItemIndex){
                    skinNameText.color = itemSO[i].playerSkin.color;
                    skinNameText.SetText(itemSO[i].name);
                    dispalyPlayerClothMat.material = itemSO[i].playerClothMat;
                    displayPlayerSkinMat.material = itemSO[i].playerSkin;
                    displayPlayerBelt.material = itemSO[i].playerBeltMat;
                }
            }
            RefreshCoinValue();
            RefreshPurchaseData();
        }
        public void RefreshCoinValue(){
            coinAmountText.SetText(playerDataSO.GetCoinValue().ToString());
        }
        private void RefreshPurchaseData(){
            for(int i = 0; i < itemSO.Length; i++){
                if(itemSO[i].GetIsItemSelected()){
                    playerDataSO.playerSkinMaterial = itemSO[i].playerSkin;
                    playerDataSO.playerClothMaterial = itemSO[i].playerClothMat;
                    playerDataSO.playerBeltMat = itemSO[i].playerBeltMat;
                    break;
                }
            }
        }

        public void ShowRight(){
            currentItemIndex++;
            if(currentItemIndex > itemSO.Length - 1){
                currentItemIndex = 0;
            }
            PlayerPrefs.SetInt("Current Index",currentItemIndex);
            ChangeView();
        }
        public void ShowLeft(){
            currentItemIndex--;
            if(currentItemIndex < 0){
                currentItemIndex = itemSO.Length - 1;
            }
            PlayerPrefs.SetInt("Current Index",currentItemIndex);
            ChangeView();
        }
        public void TryBuyItem(){
            if(itemSO[currentItemIndex].TryBuyitems(playerDataSO.GetCoinValue())){
                playerDataSO.ReduceCoins(itemSO[currentItemIndex].GetItemCost());
                playerDataSO.playerSkinMaterial = itemSO[currentItemIndex].playerSkin;
                playerDataSO.playerClothMaterial = itemSO[currentItemIndex].playerClothMat;
                playerDataSO.playerBeltMat = itemSO[currentItemIndex].playerBeltMat;
                GameEventManager.SetShopItemPurcases(itemSO[currentItemIndex].name);
                ToolTipSystem.showToolTip_static("Purchase Succesfull",Color.green);
            }else{
                ToolTipSystem.showToolTip_static("Not enough money",Color.red);
            }
            RefreshCoinValue();
            
        }
        public void TrySelectItem(){
            for (int i = 0; i < itemSO.Length; i++){
                if(currentItemIndex == i){
                    itemSO[currentItemIndex].SelectItem();
                    
                    GameEventManager.SetMostUsedSkin(itemSO[currentItemIndex].name);
                }else{
                    itemSO[i].UnSelectItem();
                }
                
            }
            RefreshPurchaseData();
        }
        private void OnApplicationQuit(){
            PlayerPrefs.SetInt("Current Index",currentItemIndex);
        }
        
        
    }
}
