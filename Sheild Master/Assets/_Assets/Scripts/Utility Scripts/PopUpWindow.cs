using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace GamerWolf.Utils {
    public class PopUpWindow : MonoBehaviour {

        [SerializeField] private iTween.EaseType popOutEaseType = iTween.EaseType.easeInOutExpo;
        [SerializeField] private iTween.EaseType popInEaseType = iTween.EaseType.easeInOutExpo;
        [SerializeField] private GameObject objectToPop;
        [SerializeField] private float speed = 1f;


        [ContextMenu("Pop Out")]
        public void PopOut(){
            gameObject.SetActive(true);
            iTween.ScaleTo(objectToPop,iTween.Hash(
                "x",1f,
                "y",1f,
                "z",1f,
                "speed",speed,
                "easy Type",popInEaseType
            ));
        }

        [ContextMenu("Pop In")]
        public void PopIn(){
            Invoke(nameof(desableShop),speed);
            iTween.ScaleTo(objectToPop,iTween.Hash(
                "x",0f,
                "y",0f,
                "z",0f,
                "speed",speed,
                "easy Type",popInEaseType
            ));
        }
        private void desableShop(){
            gameObject.SetActive(false);
        }
        
        
    }

}