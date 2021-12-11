using UnityEngine;
using UnityEngine.UI;
namespace SheildMaster {
    public class UISettings : MonoBehaviour {

        [SerializeField] private SettingsSO settingsSO;
        [SerializeField] private Color activeColor,nonActiveColor;

        [SerializeField] private Button musicButton,soundButton;

        private void Start(){
            RefreshMusicButton();
            RefreshSoundButton();
        }
        private void RefreshMusicButton(){
            if(settingsSO.GetIsMusicOn()){
                musicButton.image.color = activeColor;

            }else{
                musicButton.image.color = nonActiveColor;
            }
            
        }
        private void RefreshSoundButton(){
            if(settingsSO.GetIsSoundOn()){
                soundButton.image.color = activeColor;

            }else{
                soundButton.image.color = nonActiveColor;
            }
        }
        
        public void ChangeMusic(){
            settingsSO.SetMusic();
            RefreshMusicButton();
        }
        public void changeSound(){
            settingsSO.SetSound();
            RefreshSoundButton();
        }
        public void ChangeNotifcation(){
            settingsSO.SetNotificaiton();
        }
        public void SetNotificaiton(){
            settingsSO.SetNotificaiton();
        }
        
    }

}