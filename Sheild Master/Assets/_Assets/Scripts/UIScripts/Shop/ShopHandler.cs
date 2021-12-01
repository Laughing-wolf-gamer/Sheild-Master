using TMPro;
using UnityEngine;
using UnityEngine.UI;
using GamerWolf.Utils;
using System.Collections;
using System.Collections.Generic;
namespace SheildMaster {
    public class ShopHandler : MonoBehaviour {

        [SerializeField] private PlayerDataSO playerDataSO;

        
        [SerializeField] private Image itemView;
        [SerializeField] private TextMeshProUGUI coinAmountText;
        [SerializeField] private ShopItemSO[] itemSO;
        [SerializeField] private int currentItemIndex;
        private void Start(){
            ChangeView();
        }
        public void ChangeView(){
            for(int i = 0; i < itemSO.Length; i++){
                if(i== currentItemIndex){
                    itemView.color = itemSO[i].itemMat.color;
                }
            }
            RefreshCoinValue();
        }
        public void RefreshCoinValue(){
            coinAmountText.SetText(playerDataSO.GetCoinValue().ToString());
        }

        public void ShowRight(){
            currentItemIndex++;
            ChangeView();
        }
        public void ShowLeft(){
            currentItemIndex--;
            ChangeView();
        }
        public void TryBuyItem(){
            if(itemSO[currentItemIndex].TryBuyitems(playerDataSO.GetCoinValue())){
                playerDataSO.ReduceCoins(itemSO[currentItemIndex].GetItemCost());
                playerDataSO.currentSkinMaterial = itemSO[currentItemIndex].itemMat;
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
                }else{
                    itemSO[i].UnSelectItem();
                }
                
            }
        }
        
        
    }
}
