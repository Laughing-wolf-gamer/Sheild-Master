using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace SheildMaster {
    public class UIHandler : MonoBehaviour {
        
        [SerializeField] private PlayerDataSO playerData;
        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI[] coinAmountText;
        // [SerializeField] private TextMeshProUGUI randomAmountText;
        [SerializeField] private TextMeshProUGUI levelNumberText;
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
        public void ShowExtraLifeRewardAdWindow(bool value,int amount = 200){
            // randomAmountText.SetText(string.Concat("Watch an ad to get Extra ",amount.ToString() ," coins"));
            coinRewardWindow.gameObject.SetActive(value);
            coinRewardWindow.StartTimer();
            
        }

        public void UpdateCoinAmountUI(){
            for (int i = 0; i < coinAmountText.Length; i++){
                coinAmountText[i].SetText(playerData.GetCoinValue().ToString());
                
            }
        }
        private void SetCurrentLevel(){
            levelNumberText.SetText("Level " + playerData.GetLevelNumber().ToString());
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