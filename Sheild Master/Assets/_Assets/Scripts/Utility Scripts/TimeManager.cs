using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace GamerWolf.Utils{
    public class TimeManager : MonoBehaviour {
        [SerializeField] private UnityEvent onTimerReady;
        [SerializeField] private Button ClickButton;
        
        [SerializeField] private bool canClick;
        [SerializeField] private float msToWait = 86400000;
        [SerializeField] private TextMeshProUGUI[] timeTextArray;
        [SerializeField] private string timerName = "LastTimeClicked";
        // [SerializeField] private string timerFullString = "Reward Available in ";
        private long lastTimeClicked;


        
        
        
    
        private void Start() {
            Debug.Log(timerName + lastTimeClicked);
            if(PlayerPrefs.GetString(timerName) != string.Empty){
                lastTimeClicked = long.Parse(PlayerPrefs.GetString(timerName));
            }else{
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
                    onTimerReady?.Invoke();
                    ClickButton.interactable = true;
                    canClick = true;
                    SetTimerText("Ready!");
                    return;
                }
                long diff = ((long)DateTime.Now.Ticks - lastTimeClicked);
                long m = diff / TimeSpan.TicksPerMillisecond;
                float secondsLeft = (float)(msToWait - m) / 1000.0f;
    
                string r = "";
                //HOURS
                r += ((int)secondsLeft / 3600).ToString() + " : ";
                secondsLeft -= ((int)secondsLeft / 3600) * 3600;
                //MINUTES
                r += ((int)secondsLeft / 60).ToString("00") + " : ";
                //SECONDS
                r += (secondsLeft % 60).ToString("00");
                
                SetTimerText(string.Concat(r));
    
    
            }
        }
        private void SetTimerText(string time){
            for (int i = 0; i < timeTextArray.Length; i++){
                timeTextArray[i].SetText(time);
            }
            if(timeTextArray.Length > 1){
                if(Ready()){
                    timeTextArray[0].SetText(time);
                    timeTextArray[1].SetText("");
                }
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

        public void FiveTimeReward(){
            AdManager.dailyReward = true;
            AdManager.instance.UserChoseToWatchAd();
        }


    }
}
