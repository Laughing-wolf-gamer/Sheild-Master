using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;

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
        private AdController adController;
        private UIHandler uIHandler;
        private LevelManager levelManager;
        #endregion

        #region System Events.....
        public event EventHandler<OnGamoverEventsAargs> onGameOver;
        #endregion


        #region Singelton.........
        public static GameHandler current;
        private int randomAmountCoin;
        
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
            adController = AdController.current;
            if(adController.IsRewardedAdLoaded()){
                SetCanRewardedShowAd(true);
            }else{
                SetCanRewardedShowAd(false);
            }
            randomAmountCoin = UnityEngine.Random.Range(200,500);
            levelManager = GetComponent<LevelManager>();
            uIHandler = UIHandler.current;
            StartCoroutine(nameof(GameStartRoutine));
        }


        private IEnumerator GameStartRoutine(){
            uIHandler.ShowExtraLifeRewardAdWindow(false,0);
            onGameStart?.Invoke();
            while(!isGamePlaying){
                yield return null;
            }
            
            isGamePlaying = true;
            StartCoroutine(nameof(GamePlayRoutine));
        }
        private IEnumerator GamePlayRoutine(){
            onGamePlaying?.Invoke();
            
            while(!isGameOver){
                
                levelManager.CheckForAllEnemyDead();
                yield return null;

            }
            onGameOver?.Invoke(this,new OnGamoverEventsAargs{iswin = this.isWon});
            onGameEnd?.Invoke();
            yield return new WaitForSeconds(1.5f);
            if(canShowRewardedAds){
                int rand =  UnityEngine.Random.Range(0,5);
                if(rand >= 2){
                    uIHandler.ShowExtraLifeRewardAdWindow(true,randomAmountCoin);
                }else{
                    uIHandler.ShowExtraLifeRewardAdWindow(false);
                }
            }
            yield return new WaitForSeconds(1f);
            if(isWon){
                onWin?.Invoke();
            }else{
                if(canShowInterstetialAds){
                    adController.ShowInterstitialAd();
                }
                onLoss?.Invoke();
            }
        }
        public void RevivePlayer(){
            // Calls After Player Watches Ads.............
            AddCoin(randomAmountCoin);
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
                LevelLoader.current.PlayLevel(SceneIndex.Game_Scene);
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
            uIHandler.UpdateCoinAmountUI();
        }
        
       
        
    }

}