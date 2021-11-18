using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
namespace InkShield {
    public class OnGamoverEventsAargs :EventArgs{
        public bool iswin;
    }
    public class GameHandler : MonoBehaviour {
        
        #region Exposed Variables........

        [Header("Events")]
        [SerializeField] private PlayerDataSO playerData;
        [SerializeField] private UnityEvent onGameStart;
        [SerializeField] private UnityEvent onGamePlaying,onGamePause,onGameResume,onGameEnd,onWin,onLoss;

        [Header("Testing Variables")]
        [SerializeField] private bool isGamePlaying;
        [SerializeField] private bool isGameOver;
        [SerializeField] private bool isWon;

        [SerializeField] private bool isShowingRewardAds;
        [SerializeField] private bool canShowRewardedAds;
        [SerializeField] private bool canShowInterstetialAds;

        #endregion

        #region Private Variables.......
        private UIHandler uIHandler;
        private LevelManager levelManager;
        #endregion

        #region System Events.....
        public event EventHandler<OnGamoverEventsAargs> onGameOver;
        public bool isPlayerDead;
        #endregion


        #region Singelton.........
        public static GameHandler current;
        
        private void Awake(){
        #if UNITY_EDITOR
            Debug.unityLogger.logEnabled = true;
        #else
            Debug.unityLogger.logEnabled = false;
        #endif
            if(current == null){
                current = this;
            }else{
                Destroy(current.gameObject);
            }
        }

        #endregion




        private void Start(){
            canShowRewardedAds = AdController.current.IsRewardedAdLoaded();
            levelManager = GetComponent<LevelManager>();
            uIHandler = UIHandler.current;
            StartCoroutine(nameof(GameStartRoutine));
        }


        private IEnumerator GameStartRoutine(){
            onGameStart?.Invoke();
            while(!isGamePlaying){
                yield return null;
            }
            
            isGamePlaying = false;
            StartCoroutine(nameof(GamePlayRoutine));
        }
        private IEnumerator GamePlayRoutine(){
            onGamePlaying?.Invoke();
            while(!isGameOver){
                if(isPlayerDead){
                    if(canShowRewardedAds){
                        uIHandler.ShowExtraLifeRewardAdWindow(true);
                        if(isShowingRewardAds){
                            uIHandler.ShowExtraLifeRewardAdWindow(false);
                        }
                    }else{
                        uIHandler.ShowExtraLifeRewardAdWindow(false);
                        SetGameOver(false);
                    }
                }
                yield return null;

            }
            uIHandler.ShowExtraLifeRewardAdWindow(false);
            onGameOver?.Invoke(this,new OnGamoverEventsAargs{iswin = this.isWon});
            onGameEnd?.Invoke();
            yield return new WaitForSeconds(1.5f);
            if(isWon){
                onWin?.Invoke();
            }else{
                onLoss?.Invoke();
            }
        }
        public void RevivePlayer(){
            // Calls After Player Watches Ads.............
            isPlayerDead = false;
            LevelManager.current.OnPlayerRevive();
            AddCoin(20);
        }
        private IEnumerator ReviveRoutine(){
            yield return null;
        }

        public void PlayGame(){
            isGamePlaying = true;
            isGameOver = false;
        }
        public void SetGameOver(bool isWon){
            isGamePlaying = false;
            isGameOver = true;
            this.isWon = isWon;
        }
        public void Restart(){
            if(isGameOver){
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        public void PlayInterStetialAds(){
            if(canShowInterstetialAds){
                if(!isShowingRewardAds){
                    AdController.current.ShowInterstitialAd();
                }
            }
        }
        public void SetCanRewardedShowAd(bool value){
            // Set If Player can Watch an Reward Ad.
            canShowRewardedAds = value;
        }
        public void SetIsRewardedAdsPlaying(bool value){

            // Set If Player is Watching a Rewarded Ads....
            isShowingRewardAds = value;
        }
        public void SetCanShowInterstetialAds(bool value){
            // Set If Player can see Interstetial Ads..
            canShowInterstetialAds = value;
        }
        public void AddCoin(int value){
            playerData.AddCoins(value);
        }
        public void RemoveCoin(int value){
            playerData.ReduceCoins(value);
        }
        public void SetIsPlayerDead(bool value){
            isPlayerDead = value;
        }
        
    }

}