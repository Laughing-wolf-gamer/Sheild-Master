using TMPro;
using UnityEngine;
using UnityEngine.UI;
using GamerWolf.Utils;
namespace SheildMaster {
    public class SkinShopHandler : MonoBehaviour {

        [SerializeField] private Button itemWindowButton,skinWindowButton;
        [SerializeField] private Sprite highlightedSprite,nonHighLightedSprite;
        [SerializeField] private PlayerDataSO playerDataSO;
        [SerializeField] private Button purchaseButton,UseButton;
        [SerializeField] private TextMeshProUGUI skinNameText,itemCostText;
        [SerializeField] private Image viewImage;
        [SerializeField] private TextMeshProUGUI coinAmountText;
        [SerializeField] private ShopItemSO[] itemSO;
        [SerializeField] private int currentItemIndex;
        private void Start(){
            // if(PlayerPrefs.HasKey("Current Index")){
            //     currentItemIndex = PlayerPrefs.GetInt("Current Index");
            // }
            RefreshShop();
            
        }
        
        public void RefreshShop(){
            for(int i = 0; i < itemSO.Length; i++){
                if(i == currentItemIndex){
                    skinNameText.SetText(itemSO[i].name);
                    itemCostText.SetText(string.Concat(itemSO[i].GetItemCost().ToString()," $"));
                    viewImage.sprite = itemSO[i].itemDisplay;
                    
                }
            }
            if(itemSO[currentItemIndex].GetIsItemBought()){
                purchaseButton.gameObject.SetActive(false);
                UseButton.gameObject.SetActive(true);
                if(itemSO[currentItemIndex].GetIsItemSelected()){
                    itemCostText.SetText("Using");
                    UseButton.interactable = false;
                }else{
                    itemCostText.SetText("Purchased");
                    UseButton.interactable = true;
                }
            }else{
                purchaseButton.gameObject.SetActive(true);
                UseButton.gameObject.SetActive(false);
                // if(playerDataSO.GetCoinValue() >= itemSO[currentItemIndex].GetItemCost()){
                //     purchaseButton.interactable = true;
                // }else{
                //     purchaseButton.interactable = false;
                // }
            }
            RefreshCoinValue();
            RefreshPurchaseData();
        }
        public void OnShopClose(){
            RefreshCoinValue();
            for (int i = 0; i < itemSO.Length; i++){
                if(itemSO[i].GetIsItemSelected()){
                    PlayerPrefs.SetInt("Current Index",i);
                    break;
                }
                
            }
            if(PlayerPrefs.HasKey("Current Index")){
                currentItemIndex = PlayerPrefs.GetInt("Current Index");
            }
            RefreshShop();
            
        }
        public void OnSkinWindowOpen(){
            itemWindowButton.image.sprite = nonHighLightedSprite;
            skinWindowButton.image.sprite = highlightedSprite;
            
        }
        public void RefreshCoinValue(){
            coinAmountText.SetText(playerDataSO.GetTotalCoinValue().ToString());
        }
        private void RefreshPurchaseData(){
            for(int i = 0; i < itemSO.Length; i++){
                if(itemSO[i].GetIsItemSelected()){
                    playerDataSO.playerSkinMaterial = itemSO[i].playerSkin;
                    playerDataSO.playerClothMaterial = itemSO[i].playerCloths;
                    playerDataSO.playerBeltMat = itemSO[i].playerBelt;
                    break;
                }
            }
        }
        private void SaveCurrentItem(){
            PlayerPrefs.SetInt("Current Index",currentItemIndex);
        }
        public void ShowRight(){
            currentItemIndex++;
            if(currentItemIndex > itemSO.Length - 1){
                currentItemIndex = 0;
            }
            // SaveCurrentItem();
            RefreshShop();
        }
        public void ShowLeft(){
            currentItemIndex--;
            if(currentItemIndex < 0){
                currentItemIndex = itemSO.Length - 1;
            }
            // SaveCurrentItem();
            RefreshShop();
        }
        public void TryBuyItem(){
            if(itemSO[currentItemIndex].TryBuyitems(playerDataSO.GetTotalCoinValue())){
                AudioManager.current.PlayOneShotMusic(SoundType.Item_Purchase);
                playerDataSO.ReduceCoins(itemSO[currentItemIndex].GetItemCost());
                if(itemSO[currentItemIndex].playerSkin != null){
                    playerDataSO.playerSkinMaterial = itemSO[currentItemIndex].playerSkin;
                    playerDataSO.playerClothMaterial = itemSO[currentItemIndex].playerCloths;
                    playerDataSO.playerBeltMat = itemSO[currentItemIndex].playerBelt;
                }
                AnayltyicsManager.current.SetShopItemPurcases_UnityAnayltics(itemSO[currentItemIndex].name);
                ToolTipSystem.showToolTip_static("Purchase Succesfull",Color.green);
                if(itemSO[currentItemIndex].GetItemCost() >= 1500){
                    PlayGamesController.PostAchivements(GPGSIds.achievement_first_spend);
                }
                if(itemSO[currentItemIndex].GetItemCost() >= 5000){
                    PlayGamesController.PostAchivements(GPGSIds.achievement_big_spender);
                }
                if(itemSO[currentItemIndex].GetItemCost() >= 10000){
                    PlayGamesController.PostAchivements(GPGSIds.achievement_super_spender);
                }

            }else{
                ToolTipSystem.showToolTip_static("Not enough money",Color.red);
            }
            RefreshShop();
            RefreshCoinValue();
        }
        public void TrySelectItem(){
            for (int i = 0; i < itemSO.Length; i++){
                if(currentItemIndex == i){
                    itemSO[currentItemIndex].SelectItem();
                    AnayltyicsManager.current.SetMostUsedSkin_UnityAnayltics(itemSO[currentItemIndex].name);
                }
                else{
                    itemSO[i].UnSelectItem();
                    // if(itemSO[currentItemIndex].playerSkin != null){
                    // }
                }
                
            }
            RefreshShop();
            RefreshPurchaseData();
        }
        
        
        
    }
}
