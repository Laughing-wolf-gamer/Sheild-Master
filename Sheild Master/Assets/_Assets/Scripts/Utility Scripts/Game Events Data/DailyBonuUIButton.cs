using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace SheildMaster {
    public class DailyBonuUIButton : MonoBehaviour {
        [SerializeField] private GameObject noitificationIcon;
        [SerializeField] private Color normalColor,clamedColor;
        [SerializeField] private Image graphic;
        // [SerializeField] private TextMeshProUGUI rewardNameText,todayNameText;
        [SerializeField] private DailyRewardSO todayReward;
        [SerializeField] private PlayerDataSO playerDataSO;
        // private void Awake(){

        // }
        private void Start(){
            // graphic = GetComponent<Image>();
            CheckForDailyReward();
        }
        // public void SetTodayRewardView(DailyRewardSO rewardSO){
        //     todayReward = rewardSO;
        //     // todayNameText.SetText(string.Concat(todayReward.dayOfWeek.ToString(), " Reward"));
        //     // rewardNameText.SetText(todayReward.discription);
        // }
        
        public void CheckForDailyReward(){
            if(playerDataSO.GetIsClamedBonus()){
                noitificationIcon.SetActive(false);
            }else{
                noitificationIcon.SetActive(true);
            }
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
            noitificationIcon.SetActive(false);
            playerDataSO.SetDailyBonusAlreadyShown(true);
            playerDataSO.SetClamedBonus(true);
        }
        
        
        
    }

}