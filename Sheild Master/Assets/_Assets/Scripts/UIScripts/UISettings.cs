using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Google.Play.Review;
namespace SheildMaster {
    public class UISettings : MonoBehaviour {
        
        [SerializeField] private SettingsSO settingsSO;
        [SerializeField] private Sprite activeSprite,nonActiveSprite;
        [SerializeField] private Button musicButton,soundButton,notificationButton;
        private ReviewManager reviewManager;
        private PlayReviewInfo playReviewInfo;
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
            reviewManager = new ReviewManager();
            StartCoroutine(ReviewRoutine());
        }
        private IEnumerator ReviewRoutine(){
            yield return new WaitForSeconds(1f);
            var requestFlowOperation = reviewManager.RequestReviewFlow();
            yield return requestFlowOperation;
            if (requestFlowOperation.Error != ReviewErrorCode.NoError)
            {
                // Log error. For example, using requestFlowOperation.Error.ToString().
                yield break;
            }
            playReviewInfo = requestFlowOperation.GetResult();
            var launchFlowOperation = reviewManager.LaunchReviewFlow(playReviewInfo);
            yield return launchFlowOperation;
            playReviewInfo = null; // Reset the object
            if (launchFlowOperation.Error != ReviewErrorCode.NoError)
            {
                // Log error. For example, using requestFlowOperation.Error.ToString().
                yield break;
            }
            // The flow has finished. The API does not indicate whether the user
            // reviewed or not, or even whether the review dialog was shown. Thus, no
            // matter the result, we continue our app flow.
        }
        public void OpenLeaderboard(){
            PlayGamesController.ShowLeaderboardUI();
        }
    }

}