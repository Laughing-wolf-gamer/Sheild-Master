using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace SheildMaster {
    public class DailyBonuUIButton : MonoBehaviour {
        [SerializeField] private GameObject noitificationIcon;
        [SerializeField] private TextMeshProUGUI rewardNameText,todayNameText;
        [SerializeField] private Button clameRewardButton;
        [SerializeField] private PlayerDataSO playerDataSO;
        private DailyRewardSO todayReward;
        public void SetTodayRewardView(){
            todayNameText.SetText(string.Concat(todayReward.dayOfWeek.ToString(), " Reward"));
            rewardNameText.SetText(todayReward.discription);
            CheckForDailyReward();
        }
        public void CheckForDailyReward(){
            if(playerDataSO.GetCurentDailyBonuShown()){
                noitificationIcon.SetActive(false);
                clameRewardButton.interactable = false;
            }else{
                noitificationIcon.SetActive(true);
                clameRewardButton.interactable = true;
            }
        }

        public void setTodayReward(DailyRewardSO todayReward){
            this.todayReward = todayReward;
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
            noitificationIcon.SetActive(false);
            playerDataSO.SetDailyBonusAlreadyShown(true);
        }
        
        
        
    }

}