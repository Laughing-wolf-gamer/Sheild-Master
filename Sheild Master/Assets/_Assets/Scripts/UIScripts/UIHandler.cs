using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace SheildMaster {
    public class UIHandler : MonoBehaviour {
        
        [SerializeField] private PlayerDataSO playerData;
        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI[] levelNumberTexts;
        [SerializeField] private TextMeshProUGUI[] TotalcoinAmountText,currentLevelCoinAmountText;

        [Header("Windows")]
        [SerializeField] private GameObject abilityWindow;
        [Header("Ad window")]
        [SerializeField] private AdView coinRewardWindow;
        [Header("Images")]
        [SerializeField] private Image inkBarImage;

        [SerializeField] private TextMeshProUGUI armourAbilityCountText,KillOneEnemyCountText;

        [SerializeField] private Button armourForPlayerAbiliyButton,KillOneEnemyBeforePlayingAbiliyButton;
        [SerializeField] private AbilitySO armourForPlayerAbiliy,KillOneEnemyBeforePlayingAbiliy;

        #region Singleton......
        public static UIHandler current;

        private void Awake(){
            if(current == null){
                current = this;
            }else{
                Destroy(current.gameObject);
            }
        }
        #endregion
        private void Start(){
            SetCurrentLevel();
            UpdateCoinAmountUI();
            UpdateAbililyValueUI();
        }
        public void SetInkTankValue(float value){
            inkBarImage.fillAmount = value;
        }
        public void ShowRewardAdWindow(bool value){
            // randomAmountText.SetText(string.Concat("Watch an ad to get Extra ",amount.ToString() ," coins"));
            coinRewardWindow.gameObject.SetActive(value);
            ShowContinueButton();
            
        }
        public void ShowContinueButton(){
            coinRewardWindow.StartTimer();
        }
        public void SetCurrentLevelEarnedCoins(int coinAmont){
            for (int i = 0; i < currentLevelCoinAmountText.Length; i++){
                currentLevelCoinAmountText[i].SetText(coinAmont.ToString());
            }
        }

        public void UpdateCoinAmountUI(){
            for (int i = 0; i < TotalcoinAmountText.Length; i++){
                TotalcoinAmountText[i].SetText(playerData.GetCoinValue().ToString());
            }
        }
        private void SetCurrentLevel(){
            for (int i = 0; i < levelNumberTexts.Length; i++){
                if(playerData.GetLevelNumber() < 10){
                    levelNumberTexts[i].SetText("Level 0" + playerData.GetLevelNumber().ToString());
                }else{
                    levelNumberTexts[i].SetText("Level " + playerData.GetLevelNumber().ToString());
                }
            }
        }
        public void UpdateAbililyValueUI(){
            armourAbilityCountText.SetText(armourForPlayerAbiliy.GetAbilityUseCount().ToString());
            KillOneEnemyCountText.SetText(KillOneEnemyBeforePlayingAbiliy.GetAbilityUseCount().ToString());
            if(armourForPlayerAbiliy.GetAbilityUseCount() > 0){
                
                armourForPlayerAbiliyButton.gameObject.SetActive(true);
            }else{
                armourForPlayerAbiliyButton.gameObject.SetActive(false);
            }
            if(KillOneEnemyBeforePlayingAbiliy.GetAbilityUseCount() > 0){
                KillOneEnemyBeforePlayingAbiliyButton.gameObject.SetActive(true);
            }else{
                KillOneEnemyBeforePlayingAbiliyButton.gameObject.SetActive(false);
            }
        }
        public void EnableAbilityWindow(bool enable){
            abilityWindow.SetActive(enable);
        }
        public void UseArmourForPlayerAbiliy(){
            armourForPlayerAbiliy.UseAbility();
            LevelManager.current.ArmourForPlayer();
            UpdateAbililyValueUI();
        }
        public void UseKillOneEnemyBeforePlaying(){
            KillOneEnemyBeforePlayingAbiliy.UseAbility();
            LevelManager.current.KillOneEnemyBeforePlaying();
            UpdateAbililyValueUI();
        }
        
        public void WatchRewardedAds(){
            AdController.current.ShowRewardedAd();
        }
        public void WatchInterstetialAds(){
            AdController.current.ShowInterstitialAd();
        }
        
    }
    

}