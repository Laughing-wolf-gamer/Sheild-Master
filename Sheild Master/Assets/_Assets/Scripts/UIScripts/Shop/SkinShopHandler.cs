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
        [SerializeField] private TextMeshProUGUI skinNameText,itemCostText;
        [SerializeField] private TextMeshProUGUI coinAmountText;
        [SerializeField] private ShopItemSO[] itemSO;
        [SerializeField] private int currentItemIndex;
        [SerializeField] private SkinnedMeshRenderer dummyRenderer;

        [Header("On No Money")]
        [SerializeField] private GameObject noMoneyDialogWindow;
        [SerializeField] private Button watchAdButton;
        // [SerializeField] private TimeManager timeManager;
        private AdController adController;



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
            adController = AdController.current;
            OnSkinWindowOpen();
            noMoneyDialogWindow.SetActive(false);
            RefreshShop();
            // StartCoroutine(CheckRoutine());
        }
        // private IEnumerator CheckRoutine(){
            
        //     while(true){
        //         checkAdStatus();
        //         // CheckIfTemporaryUsing();
        //         yield return null;
        //     }
        // }
        
        // private void checkAdStatus(){
        //     if(timeManager.Ready()){
        //         Debug.Log("Button Ready To Play");
        //         watchAdButton.gameObject.SetActive(true);
        //         overlay.SetActive(false);
        //         if(adController != null){
        //             if(adController.IsRewardedAdsLoaded()){
        //                 watchAdButton.interactable = true;
        //             }else{
        //                 watchAdButton.interactable = false;
        //                 // adController.SetRewardAdsCallBack();
        //             }
        //         }
        //     }else{
        //         Debug.Log("Button Not Ready To Play");
        //         watchAdButton.interactable = false;
        //         watchAdButton.gameObject.SetActive(false);
        //         overlay.SetActive(true);
        //     }
        // }
        // private void CheckIfTemporaryUsing(){
        //     for(int i = 0; i < itemSO.Length; i++){
        //         if(i == currentItemIndex){
        //             if(!itemSO[currentItemIndex].GetIsItemBought()){
        //                 if(itemSO[currentItemIndex].isUsingTemprary){
        //                     purchaseButton.interactable = false;
        //                 }else{
        //                     purchaseButton.interactable = true;
        //                 }
        //             }
        //         }else{
        //             purchaseButton.interactable = true;
        //         }

        //     }
        // }
        public void RefreshShop(){
            for(int i = 0; i < itemSO.Length; i++){
                if(i == currentItemIndex){
                    skinNameText.SetText(itemSO[i].name);
                    itemCostText.SetText(string.Concat(itemSO[i].GetItemCost().ToString()," $"));
                    dummyRenderer.material = itemSO[i].playerSkin;
                    
                    
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
            adController.SetTryGetSkinAd(false);
            adController.AskingforExtraCoinFromShop(false);
            
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
                    playerDataSO.playerSkinMaterial = itemSO[i].playerSkin;
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
                adController.AskingforExtraCoinFromShop(false);
                adController.SetTryGetSkinAd(false);
                AudioManager.current.PlayOneShotMusic(SoundType.Item_Purchase);
                playerDataSO.ReduceCoins(itemSO[currentItemIndex].GetItemCost());
                if(itemSO[currentItemIndex].playerSkin != null){
                    playerDataSO.playerSkinMaterial = itemSO[currentItemIndex].playerSkin;
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
                adController.AskingforExtraCoinFromShop(false);
                adController.SetTryGetSkinAd(true);
            }
            RefreshShop();
            RefreshCoinValue();
            // CheckIfTemporaryUsing();
        }
        public void TrySelectItem(){
            for (int i = 0; i < itemSO.Length; i++){
                if(currentItemIndex == i){
                    itemSO[currentItemIndex].SetUsingTemproray(false);
                    itemSO[currentItemIndex].SelectItem();
                    
                    AnayltyicsManager.current.SetMostUsedSkin_UnityAnayltics(itemSO[currentItemIndex].name);
                }
                else{
                    itemSO[i].UnSelectItem();
                }
            }
            RefreshShop();
            RefreshPurchaseData();
        }
        public void TryCurrentSkinAfterAd(){
            if(!itemSO[currentItemIndex].GetIsItemBought() && !itemSO[currentItemIndex].GetIsItemSelected()){
                if(itemSO[currentItemIndex].GetItemCost() > playerDataSO.GetCashAmount()){
                    itemSO[currentItemIndex].SetUsingTemproray(true);
                    playerDataSO.SetTemprorarySkin(itemSO[currentItemIndex]);
                }
            }
        }
        public void WatchAds(){
            adController.SetTryGetSkinAd(true);
            adController.AskingforExtraCoinFromShop(false);
            adController.AskinforExtraCoinFromGame(false);
            adController.ShowRewarededAds();
            // TryCurrentSkinAfterAd(); 
        }
        
        
    }
}
