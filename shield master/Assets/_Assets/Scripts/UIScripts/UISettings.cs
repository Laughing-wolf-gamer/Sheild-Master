using UnityEngine;
using UnityEngine.UI;
namespace SheildMaster {
    public class UISettings : MonoBehaviour {
        
        [SerializeField] private SettingsSO settingsSO;
        [SerializeField] private Sprite activeSprite,nonActiveSprite;
        [SerializeField] private Button musicButton,soundButton,notificationButton;
        
        private void Start(){
            
            RefreshMusicButton();
            RefreshSoundButton();
            RefreshNoitificationButton();
        }
        private void RefreshNoitificationButton(){
            if(settingsSO.GetNotificationOn()){
                notificationButton.image.sprite = activeSprite;
            }else {
                notificationButton.image.sprite = nonActiveSprite;
            }
        }
        private void RefreshMusicButton(){
            if(settingsSO.GetIsMusicOn()){
                musicButton.image.sprite = activeSprite;
            }else{
                musicButton.image.sprite = nonActiveSprite;
            }
            
        }
        private void RefreshSoundButton(){
            if(settingsSO.GetIsSoundOn()){
                soundButton.image.sprite = activeSprite;

            }else{
                soundButton.image.sprite = nonActiveSprite;
            }
        }
        
        public void ChangeMusic(){
            settingsSO.SetMusic();
            RefreshMusicButton();
        }
        public void ChangeSound(){
            settingsSO.SetSound();
            RefreshSoundButton();
        }
        public void ChangeNotifcation(){
            settingsSO.SetNotificaiton();
            RefreshNoitificationButton();
        }
        public void ShowPrivacyAndPolicy(){
            if(settingsSO.GetPrivacyAndPolicy() != string.Empty){
                Application.OpenURL(settingsSO.GetPrivacyAndPolicy());
            }
        }
        public void OpenReviewWindow(){
            Application.OpenURL("market://details?id=com.PlayResume.SheildMaster");
        }
        
        
        public void OpenLeaderboard(){
            PlayGamesController.ShowLeaderboardUI();
        }
    }

}