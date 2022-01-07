using System;
using UnityEngine;
using GamerWolf.Utils;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.Rendering;


namespace SheildMaster {
    public class OnGamoverEventsAargs :EventArgs{
        public bool iswin;
    }
    public class GameHandler : MonoBehaviour {
        
        #region Exposed Variables........
        
        [Header("Events")]
        [SerializeField] private Volume blurVolume;
        [SerializeField] private PlayerDataSO playerData;
        [SerializeField] private UnityEvent onGameStart;
        [SerializeField] private UnityEvent onGamePlaying,onGamePause,onGameResume,onGameEnd,onWin,onLoss;
        [SerializeField] private CoinMultiplier coinMultiplier;
        [SerializeField] private TutorialManager tutorialManager;

        [Header("Testing Variables")]

        [SerializeField] private bool isGamePlaying;
        [SerializeField] private bool isGameOver;
        [SerializeField] private bool isWon;

        [SerializeField] private bool isShowingRewardAds;
        // [SerializeField] private bool canShowRewardedAds;
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
        #if UNITY_ANDROID
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            Application.targetFrameRate = 60;
        #endif
            

            CheckForAd();
            blurVolume.weight = 0f;
            Time.timeScale = 1f;
            adController = AdController.current;
            adController.askinforExtraCoinFromGame = true;
            adController.askingforExtraCoinFromShop = false;
            adController.trySkinAds = false;
            onGameResume?.Invoke();
            levelManager = GetComponent<LevelManager>();
            uIHandler = UIHandler.current;
            CheckForTutoraial();
            StartCoroutine(nameof(GameStartRoutine));
            // Invoke(nameof(CheckForTutoraial),0.5f);
            
        }
        private void CheckForTutoraial(){
            if(playerData.GetLevelNumber() == 1){
                tutorialManager.ShowInitTutorailWindow(true);
                tutorialManager.ShowCharacterTutorial(false);
            }else {
                tutorialManager.ShowInitTutorailWindow(false);
            }
            switch(playerData.GetLevelNumber()){
                case 1:
                    tutorialManager.gameObject.SetActive(true);
                    tutorialManager.ShowInitTutorailWindow(true);
                    tutorialManager.ShowCharacterTutorial(false);
                break;
                case 25:
                    tutorialManager.gameObject.SetActive(true);
                    tutorialManager.ShowCharacterTutorial(true,"Armoured Enemey","Dies with 2 hits.");
                    tutorialManager.ShowInitTutorailWindow(false);
                break;

                case 50:
                    tutorialManager.gameObject.SetActive(true);
                    tutorialManager.ShowCharacterTutorial(true,"Super Enemey","Dies with 2 hits and Bullets can break a small potion of the wall and can pass through.");
                    tutorialManager.ShowInitTutorailWindow(false);

                break;
                default:
                    tutorialManager.gameObject.SetActive(false);

                break;
            }
            // if(playerData.GetLevelNumber() == 61){
               
            // }else{
            //     tutorialManager.ShowCharacterTutorial(false);
            // }
            // if(playerData.GetLevelNumber() == 26){
            // }else{
            //     tutorialManager.ShowCharacterTutorial(false);
            // }
            // if(playerData.GetLevelNumber() > 61){
            //     tutorialManager.gameObject.SetActive(false);
            // }
        }
        private void CheckForAd(){
            
            if(adController != null){
                adController.askinforExtraCoinFromGame = true;
                adController.askingforExtraCoinFromShop = false;
                adController.trySkinAds = false;
                // if(adController.IsRewardedAdsLoaded()){
                //     SetCanRewardedShowAd(true);
                // }else{
                //     SetCanRewardedShowAd(false);
                // }
                // if(playerData.GetHasAdsInGame()){
                //     if(adController.isInterstialAdsLoaded()){
                //         SetCanShowInterstetialAds(true);
                //     }else{
                //         SetCanShowInterstetialAds(false);
                //         adController.SetInterStetialAdsCallBack();
                //     }
                // }
            }

        }
        private void Update(){
            if(Input.GetKeyDown(KeyCode.Escape)){
                OnGamePlayPause();
            }
        }
        
        public void OnGamePlayPause(){
            if(!isGameOver && isGamePlaying){
                isGamePause = !isGamePause;
                if(isGamePause){
                    blurVolume.weight = 1f;
                    Time.timeScale = 0f;
                    onGamePause?.Invoke();
                }else{
                    Time.timeScale = 1f;
                    blurVolume.weight = 0f;
                    onGameResume?.Invoke();
                }
            }
        }


        private IEnumerator GameStartRoutine(){
            Invoke(nameof(InvokeStartGame),0.2f);
            onGameStart?.Invoke();
            while(!isGamePlaying){
                yield return null;
            }
            
            isGamePlaying = true;
            StartCoroutine(nameof(GamePlayRoutine));
            
        }
        private void InvokeStartGame(){
            AudioManager.current.PlayMusic(SoundType.Game_Start);
            Debug.Log("Start The Game After 0.2f seconds");
            PlayGame();
        }
        private IEnumerator GamePlayRoutine(){
            onGamePlaying?.Invoke();
            
            while(!isGameOver){
                levelManager.CheckForAllEnemyDead();
                uIHandler.ShowRewardAdWindow(false);
                yield return null;

            }
            levelManager.SetLevelEndResult();
            onGameOver?.Invoke(this,new OnGamoverEventsAargs{iswin = this.isWon});
            onGameEnd?.Invoke();
            uIHandler.ShowRewardAdWindow(true);
            yield return new WaitForSeconds(2f);
            blurVolume.weight = 1f;
            if(isWon){
                int rand = UnityEngine.Random.Range(0,4);
                if(rand >= 0){
                    PlayInterStetialAds();
                }
                PlayGamesController.PostToLeaderboard(playerData.GetLevelNumber());
                AudioManager.current.PlayMusic(SoundType.Player_Win);
                #if !UNITY_EDITOR
                playerData.SetKillCouts(thisLevelKills);
                #endif
                onWin?.Invoke();
            }else{
                PlayInterStetialAds();
                AudioManager.current.PlayMusic(SoundType.Player_Lost);
                onLoss?.Invoke();
            }
        }
       
        
        #region Public Methods.....
        
        // public void GivePlayerTwiceCash(){
        //     // Calls After Player Watches Ads.............
        //     levelManager.AddTwiceMoney();
        // }
        
        public void IncreaseKills(){
            thisLevelKills++;
            
        }
        public void PlayGame(){
            if(!isGamePlaying){
                isGamePlaying = true;
            }
        }
        public void Restart(){
            if(isGameOver){
                Time.timeScale = 1f;
                AudioManager.current.StopAudio(SoundType.Player_Win);
                AudioManager.current.StopAudio(SoundType.Player_Lost);
                LevelLoader.current.PlayLevel(SceneIndex.Game_Scene);
            }
        }
        public void BacktoMenu(){
            Time.timeScale = 1f;
            AudioManager.current.StopAudio(SoundType.Player_Win);
            AudioManager.current.StopAudio(SoundType.Player_Lost);
            AudioManager.current.StopAudio(SoundType.Enemy_Death);
            LevelLoader.current.SwitchScene(SceneIndex.Main_Menu);
        }
        public void PlayInterStetialAds(){
            int currentLevel = playerData.GetLevelNumber();
            if((currentLevel % 2) == 0){    
                if(playerData.GetHasAdsInGame()){
                    if(canShowInterstetialAds){
                        // AdController.current.ShowInterStaialAds();
                    }
                }
            }
        }

        #endregion


        #region public Setters.......

        public void SetCanRewardedShowAd(bool value){
            // Set If Player can Watch an Reward Ad.
            // canShowRewardedAds = value;
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
            coinMultiplier.CollectCoin(value);
            uIHandler.UpdateCoinAmountUI();
        }
        public void SetGameOver(bool isWon){
            if(isGamePlaying){
                isGamePlaying = false;
                isGameOver = true;
                this.isWon = isWon;
            }
        }

        #endregion
        
       #region  Public Getters..........
        public bool GetIslost(){
            return !isWon;
        }
        public bool GetIsWon(){
            return isWon;
        }
       #endregion
        
    }

}