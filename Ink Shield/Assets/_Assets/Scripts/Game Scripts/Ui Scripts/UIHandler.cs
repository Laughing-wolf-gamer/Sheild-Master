using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace InkShield {
    public class UIHandler : MonoBehaviour {
        

        [SerializeField] private Image inkBarImage;
        [SerializeField] private GameObject extraLifeAdWindow;

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
        
        public void SetInkTankValue(float value){
            inkBarImage.fillAmount = value;
        }
        public void ShowExtraLifeRewardAdWindow(bool value){
            extraLifeAdWindow.SetActive(value);
        }
        
        
        
    }

}