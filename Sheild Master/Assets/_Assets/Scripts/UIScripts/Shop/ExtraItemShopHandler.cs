using TMPro;
using UnityEngine;
using UnityEngine.UI;
using GamerWolf.Utils;
namespace SheildMaster {
    public class ExtraItemShopHandler : MonoBehaviour {
        [SerializeField] private Sprite highLightSprite,nonHighlightedSprite;
        [SerializeField] private Button skinWindowButton,itemWindowButton;
        [SerializeField] private PlayerDataSO playerDataSO;
        [SerializeField] private TextMeshProUGUI coinAmountText;
        [SerializeField] private TextMeshProUGUI itemNameText,itemCoinCostText,itemDimondCostText;
        [SerializeField] private Image abilityShopView;
        [SerializeField] private AbilitySO[] abilityItemArray;
        [SerializeField] private int currentIndex;

        private void Start(){
            RefreshShop();
        }
        private void RefreshShop(){
            for (int i = 0; i < abilityItemArray.Length; i++){
                if(i == currentIndex){
                    itemNameText.SetText(abilityItemArray[i].name);
                    itemCoinCostText.SetText(abilityItemArray[i].cost.coinCount.ToString() + "$");
                    itemDimondCostText.SetText(abilityItemArray[i].cost.dimondCount.ToString());
                    abilityShopView.sprite = abilityItemArray[i].abilitySpriteDisplay;
                }
            }
        }
        public void OnItemWindowOpen(){
            skinWindowButton.image.sprite = nonHighlightedSprite;
            itemWindowButton.image.sprite = highLightSprite;
            
        }

        public void CheckLeft(){
            currentIndex--;
            if(currentIndex < 0){
                currentIndex = abilityItemArray.Length - 1;
            }
            RefreshShop();
        }
        public void CheckRight(){
            currentIndex++;
            if(currentIndex > abilityItemArray.Length - 1){
                currentIndex = 0;
            }
            RefreshShop();


        }
        public void TryBuyAbility(){
            if(abilityItemArray[currentIndex].CanBuyItem(playerDataSO.GetCashAmount(),playerDataSO.GetDimondCount(),2)){
                AudioManager.current.PlayOneShotMusic(SoundType.Item_Purchase);
                // ToolTipSystem.showToolTip_static("Purchase Succesfull",Color.green);
            }
        }
    }

}