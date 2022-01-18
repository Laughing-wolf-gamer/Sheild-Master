using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
namespace SheildMaster {
    public class SkinShopHandler : MonoBehaviour {

        [SerializeField] private Button itemWindowButton,skinWindowButton;
        [SerializeField] private Sprite highlightedSprite,nonHighLightedSprite;
        [SerializeField] private PlayerDataSO playerDataSO;
        [SerializeField] private Button purchaseButton,UseButton;
        [SerializeField] private GameObject usingTextOverLay;
        [SerializeField] private TextMeshProUGUI skinNameText,itemCostText;
        [SerializeField] private TextMeshProUGUI coinAmountText;
        [SerializeField] private ShopItemSO[] itemSO;
        [SerializeField] private int currentItemIndex;
        [SerializeField] private SkinnedMeshRenderer dummyRenderer;

        [Header("On No Money")]
        [SerializeField] private GameObject noMoneyDialogWindow;
        [SerializeField] private Button watchAdButton;



        #region Singleton........
        public static SkinShopHandler current{get;private set;}

        private void Awake(){
            if(current == null){
                current = this;
            }else{
                Destroy(current);
            }
        }
        #endregion
        
        private void Start(){
            OnSkinWindowOpen();
            noMoneyDialogWindow.SetActive(false);
            usingTextOverLay.SetActive(false);
            if(playerDataSO.playerSkinItem == null){
                for (int i = 0; i < itemSO.Length; i++){
                    if(itemSO[i].GetIsItemSelected()){
                        playerDataSO.playerSkinItem = itemSO[i];
                        break;
                    }
                }
            }
            RefreshShop();
        }
        
        private bool hasTemporaryItem(){
            for (int i = 0; i < itemSO.Length; i++){
                if(itemSO[i].isUsingTemprary){
                    return true;
                }
            }
            return false;
        }
        public void RefreshShop(){
            for(int i = 0; i < itemSO.Length; i++){
                if(i == currentItemIndex){
                    skinNameText.SetText(itemSO[i].name);
                    itemCostText.SetText(string.Concat(itemSO[i].GetItemCost().ToString()," CASH"));
                    dummyRenderer.material = itemSO[i].playerSkin;
                }
                
            }
            if(itemSO[currentItemIndex].GetIsItemBought()){
                itemSO[currentItemIndex].isUsingTemprary = false;
                usingTextOverLay.SetActive(false);
                purchaseButton.gameObject.SetActive(false);
                UseButton.gameObject.SetActive(true);
                if(itemSO[currentItemIndex].GetIsItemSelected()){
                    usingTextOverLay.SetActive(true);
                    itemCostText.SetText("Using");
                    UseButton.gameObject.SetActive(false);
                }else{
                    itemCostText.SetText("Purchased");
                    usingTextOverLay.SetActive(false);
                    UseButton.gameObject.SetActive(true);
                }
            }else{ 
                purchaseButton.gameObject.SetActive(true);
                UseButton.gameObject.SetActive(false);
                usingTextOverLay.gameObject.SetActive(false);
            }
            
            
            if(!itemSO[currentItemIndex].GetIsItemBought()){
                if(itemSO[currentItemIndex].isUsingTemprary){
                    purchaseButton.gameObject.SetActive(false);
                    UseButton.gameObject.SetActive(false);
                    usingTextOverLay.SetActive(true);
                }
            }
            RefreshCoinValue();
            RefreshPurchaseData();
        }
        
        public void OnShopClose(){
            noMoneyDialogWindow.SetActive(false);
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
            coinAmountText.SetText(playerDataSO.GetCashAmount().ToString());
        }
        private void RefreshPurchaseData(){
            for(int i = 0; i < itemSO.Length; i++){
                
                if(itemSO[i].GetIsItemSelected()){
                    playerDataSO.playerSkinItem = itemSO[i];
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
            RefreshShop();
        }
        public void ShowLeft(){
            currentItemIndex--;
            if(currentItemIndex < 0){
                currentItemIndex = itemSO.Length - 1;
            }
            RefreshShop();
        }
        public void TryBuyItem(){
            if(itemSO[currentItemIndex].TryBuyitems(playerDataSO.GetCashAmount())){
                AudioManager.current.PlayOneShotMusic(SoundType.Item_Purchase);
                playerDataSO.ReduceCoins(itemSO[currentItemIndex].GetItemCost());
                if(itemSO[currentItemIndex].playerSkin != null){
                    playerDataSO.playerSkinItem = itemSO[currentItemIndex];
                }
                AnayltyicsManager.current.SetShopItemPurcases_UnityAnayltics(itemSO[currentItemIndex].name);
                if(itemSO[currentItemIndex].GetItemCost() >= 200){
                    PlayGamesController.PostAchivements(GPGSIds.achievement_first_shopper);
                }
                if(itemSO[currentItemIndex].GetItemCost() >= 2000){
                    PlayGamesController.PostAchivements(GPGSIds.achievement_big_shopper);
                }
                if(itemSO[currentItemIndex].GetItemCost() >= 5000){
                    PlayGamesController.PostAchivements(GPGSIds.achievement_supper_shopper);
                    
                }
            }else{
                
                noMoneyDialogWindow.SetActive(true);

            }
            RefreshShop();
            RefreshCoinValue();
        }
        public void TrySelectItem(){
            for (int i = 0; i < itemSO.Length; i++){
                if(currentItemIndex == i){
                    itemSO[currentItemIndex].SetUsingTemproray(false);
                    itemSO[currentItemIndex].SelectItem();
                    
                    AnayltyicsManager.current.SetMostUsedSkin_UnityAnayltics(itemSO[currentItemIndex].name);
                }
                else{
                    if(itemSO[i].isUsingTemprary){
                        itemSO[i].isUsingTemprary = false;
                    }
                    itemSO[i].UnSelectItem();
                }
            }
            RefreshShop();
            RefreshPurchaseData();
        }
        public void TryCurrentSkinAfterAd(){
            for (int i = 0; i < itemSO.Length; i++){
                if(i == currentItemIndex){
                    if(!itemSO[currentItemIndex].GetIsItemBought() && !itemSO[currentItemIndex].GetIsItemSelected()){
                        if(itemSO[currentItemIndex].GetItemCost() > playerDataSO.GetCashAmount()){
                            itemSO[currentItemIndex].SetUsingTemproray(true);
                            playerDataSO.SetTemprorarySkin(itemSO[currentItemIndex]);
                        }
                    }
                }else{
                    if(itemSO[i].GetIsItemSelected()){
                        itemSO[i].UnSelectItem();
                    }
                    if(itemSO[i].isUsingTemprary){
                        itemSO[i].isUsingTemprary = false;
                    }
                }
            }
            RefreshShop();
            RefreshPurchaseData();
        }
        public void WatchAds(){
            AdManager.skinShopTry = true;
            AdManager.instance.UserChoseToWatchAd();
        }
        
        
    }
}
