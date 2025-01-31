using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace SheildMaster {
    public class DailyBonuUIButton : MonoBehaviour {
        [SerializeField] private float cashEffectSpeed = 0.1f;
        
        [SerializeField] private Color normalColor,clamedColor;
        [SerializeField] private Image graphic;
        [SerializeField] private TextMeshProUGUI rewardCashAmount;
        [SerializeField] private DailyRewardSO todayReward;
        [SerializeField] private PlayerDataSO playerDataSO;
        [SerializeField] private CoinMultiplier coinCollectersCoin,coinCollectersDimond;
        [SerializeField] private TextMeshProUGUI blackBgText;
        [SerializeField] private GameObject blackBG;
        

        
        
        public void SetIsActive(bool value){
            if(value){
                graphic.color = normalColor;
                blackBG.SetActive(false);
                
            }else{
                graphic.color = clamedColor;
                blackBG.SetActive(true);
                

            }
        }
        
        public void SetClamedText(string text){
            blackBgText.SetText(text);
        }
        
        public void ClamReward(){
            
            coinCollectersCoin.CollectCoin(todayReward.dailyBonyData.coinAmount,cashEffectSpeed);
            coinCollectersDimond.CollectDimond(todayReward.dailyBonyData.dimondAmount,cashEffectSpeed);
            playerDataSO.AddDimond(todayReward.dailyBonyData.dimondAmount);

            for (int i = 0; i < playerDataSO.abiliys.Length; i++){
                switch(playerDataSO.abiliys[i].abilityType){
                    case AbilityType.One_Bullet_Sheild:
                        playerDataSO.abiliys[i].IncreaseAbility(todayReward.dailyBonyData.shieldUseCount);
                    break;
                    case AbilityType.Kill_one_Enemy:
                        playerDataSO.abiliys[i].IncreaseAbility(todayReward.dailyBonyData.killOneEnemyUseCount);
                    break;

                }
            }
            blackBG.SetActive(true);
            SetClamedText("Claimed");
            SetIsActive(false);
            // rewardNameText.SetText("REWARD CLAMED... \n No Reward For Today.. \n Come Back Tommorow");
            // todayNameText.SetText(" ");
            // playerDataSO.SetDailyBonusAlreadyShown(true);
            playerDataSO.SetClamedBonus(true);

        }


        // This function you have to change accordingly for double.
        // It's calling in GameEventManager.
        public void ClamReward5X(){
            int multiplier = 10;
            coinCollectersCoin.CollectCoin(todayReward.dailyBonyData.coinAmount * multiplier, cashEffectSpeed);
            coinCollectersDimond.CollectDimond(todayReward.dailyBonyData.dimondAmount * multiplier, cashEffectSpeed);
            for (int i = 0; i < playerDataSO.abiliys.Length; i++){
                switch(playerDataSO.abiliys[i].abilityType){
                    case AbilityType.One_Bullet_Sheild:
                        playerDataSO.abiliys[i].IncreaseAbility(todayReward.dailyBonyData.shieldUseCount * multiplier);
                    break;
                    case AbilityType.Kill_one_Enemy:
                        playerDataSO.abiliys[i].IncreaseAbility(todayReward.dailyBonyData.killOneEnemyUseCount * multiplier);
                    break;

                }
            }
            blackBG.SetActive(true);
            SetClamedText("Claimed");
            SetIsActive(false);
            playerDataSO.SetClamedBonus(true);

        }

        public int GetCurrentRewardNumber(){
            return todayReward.dayNumber;
        }
        
        
        
        
    }

}