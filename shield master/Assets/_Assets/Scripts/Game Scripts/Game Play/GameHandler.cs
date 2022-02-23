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
        [SerializeField] private bool canShowInterstetialAds;
        [SerializeField] private bool isGamePause;
        

        #endregion

        #region Private Variables.......
        private int thisLevelKills;
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
            blurVolume.weight = 0f;
            Time.timeScale = 1f;
            onGameResume?.Invoke();
            levelManager = LevelManager.current;
            uIHandler = UIHandler.current;
            levelManager.LevelStart();
            CheckForTutoraial();
            StartCoroutine(nameof(GameStartRoutine));
            
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

                case 31:
                    tutorialManager.gameObject.SetActive(true);
                    tutorialManager.ShowCharacterTutorial(true,"Super Enemey","Dies with 2 hits and Bullets can break a small potion of the wall and can pass through.");
                    tutorialManager.ShowInitTutorailWindow(false);

                break;
                default:
                    tutorialManager.gameObject.SetActive(false);

                break;
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
            CameraMultiTarget.current.SetTargets(PlayerController.player.transform);
            Invoke(nameof(InvokeStartGame),0.3f);
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
            levelManager.StartFlicker();
            onGameOver?.Invoke(this,new OnGamoverEventsAargs{iswin = this.isWon});
            onGameEnd?.Invoke();
            uIHandler.ShowRewardAdWindow(true);
            yield return new WaitForSeconds(2f);
            blurVolume.weight = 1f;
            PlayAds();
            if(isWon){

                PlayGamesController.PostToLeaderboard(playerData.GetLevelNumber());
                AudioManager.current.PlayMusic(SoundType.Player_Win);
                #if !UNITY_EDITOR
                playerData.SetKillCouts(thisLevelKills);
                #endif
                onWin?.Invoke();
                if(playerData.GetLevelNumber() == 1 || playerData.GetLevelNumber() == 25 || playerData.GetLevelNumber() == 50){
                    AnayltyicsManager.current.SetTutoraialDataAnalytics();
                }
                AnayltyicsManager.current.SetLevelUpResult(playerData.GetLevelNumber());
            }else{
                AudioManager.current.PlayMusic(SoundType.Player_Lost);
                onLoss?.Invoke();
            }
            
        }
       
        
        #region Public Methods.....
        
        
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
                AdManager.instance.DestroyBanner();
            }
        }
        public void BacktoMenu(){
            Time.timeScale = 1f;
            AudioManager.current.StopAudio(SoundType.Player_Win);
            AudioManager.current.StopAudio(SoundType.Player_Lost);
            AudioManager.current.StopAudio(SoundType.Enemy_Death);
            LevelLoader.current.SwitchScene(SceneIndex.Main_Menu);
            AdManager.instance.DestroyBanner();
        }
        public void PlayAds(){
            int currentLevel = playerData.GetLevelNumber();
            if(playerData.GetHasAdsInGame()){
                AdManager.instance.RequestBanner();
                if(currentLevel < 4){
                    if((currentLevel % 2) == 0){
                        Debug.Log("Playing Interstial Ads");
                        AdManager.instance.GameOver();
                    }
                }else{
                    AdManager.instance.GameOver();
                }
            }
        }

        #endregion


        #region public Setters.......
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