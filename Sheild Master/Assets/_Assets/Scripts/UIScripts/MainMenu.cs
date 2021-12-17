using TMPro;
using UnityEngine;
using UnityEngine.Events;
namespace SheildMaster {
    public class MainMenu : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI coinAmount;
        [SerializeField] private UnityEvent onGameStart;
        [SerializeField] private GameObject quitWindow,shopWindow,dailyRewardWindow;
        [SerializeField] private PlayerDataSO playerData;
        private void Start(){
            AdController.current.askingforExtraCoinFromShop = true;
            onGameStart?.Invoke();
            RefreshCoinValue();
            AudioManager.current.PlayMusic(SoundType.BGM);
            playerData.onCurrencyAmountChange += RefreshCoinValue;
        }
        public void PlayGame(){
            AudioManager.current.StopAudio(SoundType.BGM);
            LevelLoader.current.PlayLevel(SceneIndex.Game_Scene);
            AdController.current.askingforExtraCoinFromShop = false;
        }
        
        private void Update(){
               
            if(Input.GetKeyDown(KeyCode.Escape)){
                if(CanQuit()){
                    quitWindow.SetActive(true);
                }
            }
        }
        private bool CanQuit(){
            return !(shopWindow.activeInHierarchy || dailyRewardWindow.activeInHierarchy);
        }
        public void Quit(){
            Application.Quit();
        }
        public void RefreshCoinValue(){
            coinAmount.SetText(string.Concat(playerData.GetTotalCoinValue().ToString()));
        }
        
        
    }

}