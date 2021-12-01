using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace GamerWolf.Utils {
    public class PopUpWindow : MonoBehaviour {

        [SerializeField] private iTween.EaseType popOutEaseType = iTween.EaseType.easeInOutExpo;
        [SerializeField] private iTween.EaseType popInEaseType = iTween.EaseType.easeInOutExpo;
        [SerializeField] private GameObject objectToPop;
        [SerializeField] private float delay = 1f;


        [ContextMenu("Pop Out")]
        public void PopOut(){
            // gameObject.SetActive(true);
            // while(transform.localScale != Vector3.one){
            //     iTween.ScaleTo(objectToPop,iTween.Hash(
            //         "x",1f,
            //         "y",1f,
            //         "z",1f,
            //         "speed",delay,
            //         "easy Type",popInEaseType
            //     ));
            // }
        }

        [ContextMenu("Pop In")]
        public void PopIn(){
            // while(transform.localScale != Vector3.zero){
            //     iTween.ScaleTo(objectToPop,iTween.Hash(
            //         "x",0f,
            //         "y",0f,
            //         "z",0f,
            //         "speed",delay,
            //         "easy Type",popInEaseType
            //     ));
            // }
            // gameObject.SetActive(false);
        }
        
        
    }

}