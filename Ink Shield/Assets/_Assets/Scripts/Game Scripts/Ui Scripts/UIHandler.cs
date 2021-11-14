using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace InkShield {
    public class UIHandler : MonoBehaviour {
        

        [SerializeField] private Image inkBarImage;


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
        
    }

}