using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;
namespace SheildMaster {
    public class AdView : MonoBehaviour {

        [SerializeField] private Button cancleButton;
        [SerializeField] private UnityEvent onOpen,onClose;
        private float maxTimer = 2f;
        private bool startTimer;
        private void Start(){
            cancleButton.gameObject.SetActive(false);
            maxTimer = 2f;
        }
        public void StartTimer(){
            onOpen?.Invoke();
            startTimer = true;
        }
        private void Update(){
            if(startTimer){
                maxTimer -= Time.deltaTime;
                if(maxTimer <= 0f){
                    maxTimer = 0f;
                    cancleButton.gameObject.SetActive(true);
                    startTimer = false;
                }
            }
        }
        public void InovkeOnClose(){
            onClose?.Invoke();
        }
        
        
    }
}
