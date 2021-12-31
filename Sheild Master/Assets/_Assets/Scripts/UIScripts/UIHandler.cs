using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace SheildMaster {
    public class UIHandler : MonoBehaviour {
        
        [SerializeField] private PlayerDataSO playerData;
        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI[] levelNumberTexts;
        [SerializeField] private TextMeshProUGUI[] TotalcoinAmountText,currentLevelCoinAmountText/*dimondTextButton*/;

        [Header("Windows")]
        [SerializeField] private GameObject abilityWindow;
        [Header("Ad window")]
        [SerializeField] private GameObject adRewardButton;
        [Header("Images")]
        [SerializeField] private Image inkBarImage;

        [SerializeField] private TextMeshProUGUI armourAbilityCountText,KillOneEnemyCountText;
        [SerializeField] private GameObject armourAbilityCountTextObject,killAbilityCountObject; 

        [SerializeField] private Button armourForPlayerAbiliyButton,KillOneEnemyBeforePlayingAbiliyButton;
        [SerializeField] private GameObject notUnlockViewForSheild,notUnlockViewForKillOne;
        [SerializeField] private AbilitySO armourForPlayerAbiliy,KillOneEnemyBeforePlayingAbiliy;

        #region Singleton......
        public static UIHandler current;

        private void Awake(){
            if(current == null){
                current = this;
            }else{
                Destroy(current);
            }
        }
        #endregion

        private void Start(){

            SetCurrentLevel();
            UpdateCoinAmountUI();
            playerData.onCurrencyValueChange += UpdateCoinAmountUI;
            playerData.onDimondValueChange += UpdateCoinAmountUI;
            UpdateAbililyValueUI();
        }
        public void SetInkTankValue(float value){
            inkBarImage.fillAmount = value;
        }
        public void ShowRewardAdWindow(bool value){
            adRewardButton.SetActive(value);
        }
        
        public void SetCurrentLevelEarnedCoins(int coinAmont){
            for (int i = 0; i < currentLevelCoinAmountText.Length; i++){
                currentLevelCoinAmountText[i].SetText(coinAmont.ToString());
            }
            
        }

        public void UpdateCoinAmountUI(){
            for (int i = 0; i < TotalcoinAmountText.Length; i++){
                TotalcoinAmountText[i].SetText(playerData.GetCashAmount().ToString());
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
        public void HideSheildAbilityIcon(bool hide){
            notUnlockViewForSheild.SetActive(hide);
            if(!hide){
                armourAbilityCountTextObject.SetActive(true);
                armourForPlayerAbiliyButton.interactable = true;
            }else{
                armourAbilityCountTextObject.SetActive(false);
                armourForPlayerAbiliyButton.interactable = false;
            }

        }
        public void HideKillAbilityIcon(bool hide){
            notUnlockViewForKillOne.SetActive(hide);
            if(!hide){
                killAbilityCountObject.SetActive(true);
                KillOneEnemyBeforePlayingAbiliyButton.interactable = true;
            }else{
                killAbilityCountObject.SetActive(false);
                KillOneEnemyBeforePlayingAbiliyButton.interactable = false;
            }
        }
        public void UpdateAbililyValueUI(){
            armourAbilityCountText.SetText(armourForPlayerAbiliy.GetAbilityUseCount().ToString());
            KillOneEnemyCountText.SetText(KillOneEnemyBeforePlayingAbiliy.GetAbilityUseCount().ToString());
        }
        public void EnableAbilityWindow(bool enable){
            abilityWindow.SetActive(enable);
        }
        public void UseArmourForPlayerAbiliy(){
            if(armourForPlayerAbiliy.CanUseAbility()){
                LevelManager.current.ArmourForPlayer();
            }else{
                HideSheildAbilityIcon(true);
            }
            armourForPlayerAbiliy.UseAbility();
            UpdateAbililyValueUI();
        }
        public void UseKillOneEnemyBeforePlaying(){
            KillOneEnemyBeforePlayingAbiliy.UseAbility();
            if(KillOneEnemyBeforePlayingAbiliy.CanUseAbility()){
                LevelManager.current.KillOneEnemyBeforePlaying();
            }else{
                HideKillAbilityIcon(true);
            }
            UpdateAbililyValueUI();
        }
        
        public void WatchRewardedAds(){
            AdController.current.ShowRewarededAds();
        }
        
        public void PauseMusic(){
            AudioManager.current.PauseMusic(SoundType.BGM);
        }
    }
    

}