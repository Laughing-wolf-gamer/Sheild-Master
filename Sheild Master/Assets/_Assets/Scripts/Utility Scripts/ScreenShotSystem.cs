using UnityEngine;
using UnityEngine.Events;
using System.Collections;
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
                TakeScreenShot();
            }
        }
        private void InvoekAfterScreenShot(){
            screenShotCount++;
            PlayerPrefs.SetInt("Screen Shot Count",screenShotCount);
            onAfterScreenShot?.Invoke();
        }
        [ContextMenu("Take Screen Shot")]
        public void TakeScreenShot(){
            StartCoroutine(ScreenShotRoutine());
        }
        private IEnumerator ScreenShotRoutine(){
            yield return new WaitForEndOfFrame();
            int width = Screen.width;
            int height = Screen.height;
            Texture2D screenShotTexture = new Texture2D(width,height,TextureFormat.ARGB32,false);
            Rect rect = new Rect(0,0,width,height);
            screenShotTexture.ReadPixels(rect,0,0);
            screenShotTexture.Apply();
            byte[] byteArray = screenShotTexture.EncodeToPNG();
            System.IO.File.WriteAllBytes(Application.dataPath + "/Screen Shots.png",byteArray);
        }
    }

}