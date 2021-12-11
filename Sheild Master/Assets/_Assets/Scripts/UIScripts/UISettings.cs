using UnityEngine;
using UnityEngine.UI;
namespace SheildMaster {
    public class UISettings : MonoBehaviour {

        [SerializeField] private SettingsSO settingsSO;
        [SerializeField] private Sprite activeSprite,nonActiveSprite;

        [SerializeField] private Button musicButton,soundButton;

        private void Start(){
            RefreshMusicButton();
            RefreshSoundButton();
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
        }
        public void SetNotificaiton(){
            settingsSO.SetNotificaiton();
        }
        
    }

}