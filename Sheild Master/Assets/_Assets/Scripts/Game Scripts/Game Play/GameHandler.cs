using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace SheildMaster {
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
        [SerializeField] private bool isGamePause;

        #endregion

        #region Private Variables.......
        private int thisLevelKills;
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
        private int[] coinAmountArray = new int[]{25,30,35,40,45,50};
        private void Awake() {
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
            onGameResume?.Invoke();
            if(adController != null){
                if(adController.IsRewardedAdLoaded()){
                    SetCanRewardedShowAd(true);
                }else{
                    SetCanRewardedShowAd(false);
                }
                if(playerData.GetHasAdsInGame()){
                    if(adController.isInterstetialAdLoaded()){
                        SetCanShowInterstetialAds(true);
                    }else{
                        SetCanShowInterstetialAds(false);
                    }
                }
            }

            
            int rand = UnityEngine.Random.Range(0,coinAmountArray.Length);
            randomAmountCoin = coinAmountArray[rand];

            levelManager = GetComponent<LevelManager>();
            uIHandler = UIHandler.current;
            StartCoroutine(nameof(GameStartRoutine));
            PlayerInputController.current.OnGamePause += OnGamePlayPause;
        }
        private void OnGamePlayPause(){
            if(!isGameOver && isGamePlaying){
                isGamePause = !isGamePause;
                if(isGamePause){
                    Time.timeScale = 0f;
                    onGamePause?.Invoke();
                }else{
                    Time.timeScale = 1f;
                    onGameResume?.Invoke();
                }
            }
        }


        private IEnumerator GameStartRoutine(){
            // uIHandler.ShowExtraLifeRewardAdWindow(false,0);
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
            levelManager.SetLevelEndResult();
            onGameOver?.Invoke(this,new OnGamoverEventsAargs{iswin = this.isWon});
            onGameEnd?.Invoke();
            if(canShowRewardedAds){
                yield return new WaitForSeconds(0.5f);
                uIHandler.ShowExtraLifeRewardAdWindow(true,randomAmountCoin);
                // int rand =  UnityEngine.Random.Range(0,5);
                // if(rand >= 2){
                // }else{
                //     uIHandler.ShowExtraLifeRewardAdWindow(false);
                // }
            }
            yield return new WaitForSeconds(1f);
            if(isWon){
                #if !UNITY_EDITOR
                playerData.SetKillCouts(thisLevelKills);
                #endif
                onWin?.Invoke();
            }else{
                if(playerData.GetHasAdsInGame()){
                    if(canShowInterstetialAds){
                        adController.ShowInterstitialAd();
                    }
                }
                onLoss?.Invoke();
            }
        }
       
        
        #region Public Methods.....
        
        public void RevivePlayer(){
            // Calls After Player Watches Ads.............
            AddCoin(randomAmountCoin);
        }
        
        public void IncreaseKills(){
            thisLevelKills++;
            
        }
        public void PlayGame(){
            isGamePlaying = true;
            isGameOver = false;
        }
        public void Restart(){
            if(isGameOver){
                LevelLoader.current.PlayLevel(SceneIndex.Game_Scene);
            }
        }
        public void BacktoMenu(){
            LevelLoader.current.SwitchScene(SceneIndex.Main_Menu);
        }
        public void PlayInterStetialAds(){
            if(playerData.GetHasAdsInGame()){
                if(canShowInterstetialAds){
                    if(!isShowingRewardAds){
                        UIHandler.current.WatchInterstetialAds();
                    }
                }
            }
        }

        #endregion


        #region public Setters.......

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
        public void SetGameOver(bool isWon){
            isGamePlaying = false;
            isGameOver = true;
            this.isWon = isWon;
        }

        #endregion
        
       
        
    }

}