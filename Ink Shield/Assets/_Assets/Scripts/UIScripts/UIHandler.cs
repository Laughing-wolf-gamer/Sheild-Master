using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace InkShield {
    public class UIHandler : MonoBehaviour {
        
        [SerializeField] private PlayerDataSO playerData;
        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI coinAmountText;
        [SerializeField] private TextMeshProUGUI randomAmountText;
        [SerializeField] private TextMeshProUGUI levelNumberText;
        [Header("Windows")]
        [SerializeField] private GameObject abilityWindow;
        [SerializeField] private GameObject coinRewardWindow;
        [Header("Images")]
        [SerializeField] private Image inkBarImage;

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
            randomAmountText.SetText("Get Extra " + amount.ToString());
            coinRewardWindow.SetActive(value);
        }
        public void UpdateCoinAmountUI(){
            coinAmountText.SetText(playerData.GetCoinValue().ToString());
        }
        private void SetCurrentLevel(){
            levelNumberText.SetText("Level " + playerData.GetLevelNumber().ToString());
        }
        public void UpdateAbililyValueUI(){
            armourForPlayerAbiliyButton.image.fillAmount = armourForPlayerAbiliy.GetAbilityValueNormalized();
            KillOneEnemyBeforePlayingAbiliyButton.image.fillAmount = KillOneEnemyBeforePlayingAbiliy.GetAbilityValueNormalized();
            if(armourForPlayerAbiliyButton.image.fillAmount >= 1){
                armourForPlayerAbiliyButton.interactable = true;
            }else{
                armourForPlayerAbiliyButton.interactable = false;
            }
            if(KillOneEnemyBeforePlayingAbiliyButton.image.fillAmount >= 1){
                KillOneEnemyBeforePlayingAbiliyButton.interactable = true;
            }else{
                KillOneEnemyBeforePlayingAbiliyButton.interactable = false;
            }
        }
        public void EnableAbilityWindw(bool enable){
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
        
        
        
    }
    

}