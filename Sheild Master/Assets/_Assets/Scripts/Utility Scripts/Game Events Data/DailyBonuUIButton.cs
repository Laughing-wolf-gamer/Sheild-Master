using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace SheildMaster {
    public class DailyBonuUIButton : MonoBehaviour {
        
        [SerializeField] private Color normalColor,clamedColor;
        [SerializeField] private Image graphic;
        [SerializeField] private TextMeshProUGUI rewardCashAmount;
        [SerializeField] private DailyRewardSO todayReward;
        [SerializeField] private PlayerDataSO playerDataSO;
        private void Start(){
            SetTodayRewardView();
        }
        public void SetTodayRewardView(){
            // rewardCashAmount.SetText(todayReward.discription);
        }
        
        
        public void SetIsActive(bool value){
            if(value){
                graphic.color = normalColor;
            }else{
                graphic.color = clamedColor;
            }
        }
        

        
        public void ClamReward(){
            playerDataSO.AddCoins(todayReward.dailyBonyData.coinAmount);
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
            SetIsActive(false);
            // rewardNameText.SetText("REWARD CLAMED... \n No Reward For Today.. \n Come Back Tommorow");
            // todayNameText.SetText(" ");
            playerDataSO.SetDailyBonusAlreadyShown(true);
            playerDataSO.SetClamedBonus(true);
        }
        
        
        
    }

}