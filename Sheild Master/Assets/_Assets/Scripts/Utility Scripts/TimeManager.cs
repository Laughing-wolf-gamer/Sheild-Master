using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace GamerWolf.Utils{
    public class TimeManager : MonoBehaviour {

        [SerializeField] private Button ClickButton;
        [SerializeField] private bool canClick;
        [SerializeField] private float msToWait = 86400000;
        [SerializeField] private TextMeshProUGUI timeText;
        [SerializeField] private string timerName = "LastTimeClicked";
        [SerializeField] private string timerFullString = "Reward Available in ";
        private long lastTimeClicked;


        
        
        
    
        private void Start() {
            Debug.Log(timerName + lastTimeClicked);
            if(PlayerPrefs.GetString(timerName) != string.Empty){
                lastTimeClicked = long.Parse(PlayerPrefs.GetString(timerName));
            }else{
                // timeText.SetText("Ready!");
                lastTimeClicked = 0;
                PlayerPrefs.SetString(timerName, lastTimeClicked.ToString());
            }
    
    
            if (!Ready()){
                canClick = false;
                ClickButton.interactable = false;
            }
        }
    
        private void Update(){
            if (!ClickButton.IsInteractable()){
                if (Ready()){
                    ClickButton.interactable = true;
                    canClick = true;
                    timeText.SetText("Ready!");
                    return;
                }
                long diff = ((long)DateTime.Now.Ticks - lastTimeClicked);
                long m = diff / TimeSpan.TicksPerMillisecond;
                float secondsLeft = (float)(msToWait - m) / 1000.0f;
    
                string r = "";
                //HOURS
                r += ((int)secondsLeft / 3600).ToString() + "h ";
                secondsLeft -= ((int)secondsLeft / 3600) * 3600;
                //MINUTES
                r += ((int)secondsLeft / 60).ToString("00") + "m ";
                //SECONDS
                r += (secondsLeft % 60).ToString("00") + "s";
                timeText.text = string.Concat(timerFullString ," " ,r) ;
    
    
            }
        }

    
    
        public void Click() {
            lastTimeClicked = (long)DateTime.Now.Ticks;
            PlayerPrefs.SetString(timerName, lastTimeClicked.ToString());
            canClick = false;
        }
        public bool Ready(){
            long diff = ((long)DateTime.Now.Ticks - lastTimeClicked);
            long m = diff / TimeSpan.TicksPerMillisecond;
    
            float secondsLeft = (float)(msToWait - m) / 1000.0f;
    
            if (secondsLeft < 0){
                //DO SOMETHING WHEN TIMER IS FINISHED
                return true;
            }
    
            return false;
        }
        
        
    }
}
