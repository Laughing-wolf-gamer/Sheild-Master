using UnityEngine;


namespace SheildMaster {
    public class PlayButtonClickSound : MonoBehaviour {


        private AudioManager audioManager;


        private void Start(){
            audioManager = AudioManager.current;
        }
        public void PlayButtonClick(){
            if(audioManager != null){
                audioManager.PlayMusic(SoundType.ButtonClickSound);
            }
        }        
        
    }

}