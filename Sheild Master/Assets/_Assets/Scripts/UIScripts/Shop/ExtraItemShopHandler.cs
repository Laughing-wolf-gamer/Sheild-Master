using TMPro;
using UnityEngine;
using UnityEngine.UI;
using GamerWolf.Utils;
using System.Collections;
using System.Collections.Generic;
namespace SheildMaster {
    public class ExtraItemShopHandler : MonoBehaviour {

        // [SerializeField] private PlayerDataSO playerDataSO;
        // [SerializeField] private Button purchaseButton,UseButton;
        // [SerializeField] private TextMeshProUGUI coinAmountText;
        // [SerializeField] private MeshRenderer accessoryMeshREnderer;
        // [SerializeField] private SkinnedMeshRenderer dispalyPlayerClothMat,displayPlayerBelt;

        // [SerializeField] private ShopItemSO[] itemSOs;
        // private int currentIndex;
        // private void RefershShop(){
        //     for (int i = 0; i < itemSOs.Length; i++){
        //         if(i == currentIndex){
        //             if(itemSOs[i].shopItemType == ShopItemType.ITEMS){
        //                 dispalyPlayerClothMat.material = itemSOs[i].playerClothMat;
        //                 displayPlayerBelt.material = itemSOs[i].playerBeltMat;
        //                 accessoryMeshREnderer.gameObject.SetActive(false);
        //             }
        //             if(itemSOs[i].shopItemType == ShopItemType.Accessories){

        //                 accessoryMeshREnderer.material = itemSOs[i].ForceFieldMat;
        //                 accessoryMeshREnderer.gameObject.SetActive(true);
        //             }
        //         }
        //     }
        // }
        // public void RefreshCoinValue(){
        //     coinAmountText.SetText(playerDataSO.GetCoinValue().ToString());
        // }
        // private void RefreshPurchaseData(){
        //     for(int i = 0; i < itemSOs.Length; i++){
        //         if(itemSOs[i].GetIsItemSelected()){
        //             if(itemSOs[i].shopItemType== ShopItemType.ITEMS){
        //                 playerDataSO.playerClothMaterial = itemSOs[i].playerClothMat;
        //                 playerDataSO.playerBeltMat = itemSOs[i].playerBeltMat;
        //                 accessoryMeshREnderer.gameObject.SetActive(false);
        //             }
        //             if(itemSOs[i].shopItemType == ShopItemType.Accessories){
        //                 accessoryMeshREnderer.material = itemSOs[i].ForceFieldMat;
        //                 accessoryMeshREnderer.gameObject.SetActive(true);
        //             }
        //             break;
        //         }
        //     }
        // }

        // public void ShowRight(){
        //     currentIndex++;
        //     if(currentIndex > itemSOs.Length - 1){
        //         currentIndex = 0;
        //     }
        //     PlayerPrefs.SetInt("Current Index",currentIndex);
        //     RefershShop();
        // }
        // public void ShowLeft(){
        //     currentIndex--;
        //     if(currentIndex < 0){
        //         currentIndex = itemSOs.Length - 1;
        //     }
        //     PlayerPrefs.SetInt("Current Index",currentIndex);
        //     RefershShop();
        // }
        // public void TryBuyItem(){
        //     if(itemSOs[currentIndex].TryBuyitems(playerDataSO.GetCoinValue())){
        //         playerDataSO.ReduceCoins(itemSOs[currentIndex].GetItemCost());
        //         if(itemSOs[currentIndex].shopItemType== ShopItemType.ITEMS){
        //             playerDataSO.playerClothMaterial = itemSOs[currentIndex].playerClothMat;
        //             playerDataSO.playerBeltMat = itemSOs[currentIndex].playerBeltMat;

        //         }
        //         if(itemSOs[currentIndex].shopItemType == ShopItemType.Accessories){
        //             accessoryMeshREnderer.material = itemSOs[currentIndex].ForceFieldMat;
        //         }
        //         GameEventManager.SetShopItemPurcases(itemSOs[currentIndex].name);
        //         ToolTipSystem.showToolTip_static("Purchase Succesfull",Color.green);
        //     }else{
        //         ToolTipSystem.showToolTip_static("Not enough money",Color.red);
        //     }
        //     RefershShop();
        //     RefreshCoinValue();
        // }
        // public void TrySelectItem(){
        //     for (int i = 0; i < itemSOs.Length; i++){
        //         if(currentIndex == i){
        //             itemSOs[currentIndex].SelectItem();
        //             GameEventManager.SetMostUsedSkin(itemSOs[currentIndex].name);

        //         }else{
        //             itemSOs[i].UnSelectItem();
        //         }
                
        //     }
        //     RefershShop();
        //     RefreshPurchaseData();
        // }
        

    }

}