using TMPro;
using UnityEngine;
using GamerWolf.Utils;
using System.Collections;
using System.Collections.Generic;
namespace SheildMaster{
    public class CoinMultiplier : MonoBehaviour {
        [SerializeField] private bool isCoinMultiplier = true;

        [SerializeField] private PlayerDataSO playerData;
        [SerializeField] private TextMeshProUGUI coinAmountText;
        [SerializeField] private UICoinParitcalEffect uICoinParitcalEffect;

        public void CollectCoin(int coinValue,float increaseSpeed = 0.1f){
            StartCoroutine(CollectionRoutine(coinValue,increaseSpeed));
            if(coinValue > 0){
                uICoinParitcalEffect.FromTo();
            }
        }
        public void CollectDimond(int dimondValue,float increaseSped = 0.1f){
            if(dimondValue > 0){
                uICoinParitcalEffect.FromTo();
            }
            StartCoroutine(CollectionRoutine(dimondValue,increaseSped));
        }
        
        private IEnumerator CollectionRoutine(int coinValue,float increaseTime){
            int currentValue = playerData.GetTotalCoinValue();
            coinAmountText.SetText(currentValue.ToString());
            int totalvalue = playerData.GetTotalCoinValue() + coinValue;
            if(isCoinMultiplier){
                playerData.AddCoins(coinValue);
            }else{
                playerData.AddDimond(coinValue);
            }
            if(coinValue > 0){
                while(currentValue != totalvalue){
                    currentValue++;
                    coinAmountText.SetText(currentValue.ToString());
                    yield return new WaitForSeconds(increaseTime);
                }
            }
            
            currentValue = 0;
            
        }
        
    }

}