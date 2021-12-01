using UnityEngine;
using UnityEngine.Events;

namespace GamerWolf.Utils {
    public class ScreenShotSystem : MonoBehaviour {
        [SerializeField] private UnityEvent onBeforScreenShot,onAfterScreenShot;
        public static ScreenShotSystem current;
        private int screenShotCount = 1;
        private void Awake(){
            if(current == null){
                current = this;
            }else{
                Destroy(current.gameObject);
            }
            DontDestroyOnLoad(current.gameObject);
        }
        private void Start(){
            screenShotCount = PlayerPrefs.GetInt("Screen Shot Count");
        }
        private void Update(){
            if(Input.GetKeyDown(KeyCode.T)){
                onBeforScreenShot?.Invoke();
                ScreenCapture.CaptureScreenshot(string.Concat("Game Screen Shot",screenShotCount,".png"));
                Invoke(nameof(InvoekAfterScreenShot),0.1f);
            }
        }
        private void InvoekAfterScreenShot(){
            screenShotCount++;
            PlayerPrefs.SetInt("Screen Shot Count",screenShotCount);
            onAfterScreenShot?.Invoke();
        }
        
    }

}