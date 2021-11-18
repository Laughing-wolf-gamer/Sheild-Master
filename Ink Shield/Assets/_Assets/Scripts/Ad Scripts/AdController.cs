using System;
using UnityEngine;
using GoogleMobileAds.Api;


namespace InkShield{

    public class AdController : MonoBehaviour{
        public static AdController current;

        private readonly string interstitialId = "ca-app-pub-3940256099942544/1033173712";
        private readonly string rewardedId = "ca-app-pub-3940256099942544/5224354917";


        private readonly string appId = "ca-app-pub-1447736674902262~2052801395";

        private InterstitialAd interstitialAd;
        private RewardedAd rewardedAd;

        // private int npa;

        
        private void Awake(){
            if (current == null){
                current = this;
            }
            else {
                Destroy(gameObject);

            }
            DontDestroyOnLoad(this.gameObject);
            MobileAds.Initialize(initStatus => {
                Debug.Log("Init Stauts " + initStatus);
            });
        }



        private void Start(){
            InitializeAd();
        }

        

        private void InitializeAd(){
            interstitialAd?.Destroy();
            interstitialAd = new InterstitialAd(interstitialId);
            interstitialAd.OnAdClosed += (sender, args) => RequestInterstitial();
            interstitialAd.OnAdFailedToLoad += HandleInterStetailAdFailedToLoad;
            interstitialAd.OnAdFailedToShow += HandleinterstitialAdAdFailedToShow;
            interstitialAd.OnAdLoaded += HandleInterStetialAdLoaded;
            interstitialAd.OnAdOpening += HandleInterstitialAdAdOpening;

            RequestInterstitial();


            rewardedAd = new RewardedAd(rewardedId);
            rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
            rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
            rewardedAd.OnAdOpening += HandleRewardedAdOpening;
            rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
            rewardedAd.OnAdClosed += HandleRewardedAdClosed;
            RequestRewardedAd();
        }

        

        private void RequestInterstitial(){
            AdRequest request = new AdRequest.Builder().Build();
            interstitialAd.LoadAd(request);
        }

        private void RequestRewardedAd(){
            AdRequest request = new AdRequest.Builder().Build();
            rewardedAd.LoadAd(request);
        }
        
        public void ShowInterstitialAd(){
            if (interstitialAd.IsLoaded()){
                interstitialAd.Show();
            }
            else{
                RequestInterstitial();
                if (interstitialAd.IsLoaded()){
                    interstitialAd.Show();
                }
                else {
                    Debug.Log("Interstitial is not loaded");
                }
            }
        }

        public bool IsRewardedAdLoaded(){
            return rewardedAd.IsLoaded();
        }

        public void ShowRewardedAd(){
            if (rewardedAd.IsLoaded()){
                rewardedAd.Show();
                GameHandler.current.SetIsRewardedAdsPlaying(true);
            }else{
                RequestRewardedAd();
                if (rewardedAd.IsLoaded()){
                    rewardedAd.Show();
                    GameHandler.current.SetIsRewardedAdsPlaying(true);
                }
                else{
                    GameHandler.current.SetIsRewardedAdsPlaying(false);
                }
            }
        }

        #region RewardedHandle

        public void HandleRewardedAdLoaded(object sender, EventArgs args){
            if(GameHandler.current != null){
                GameHandler.current.SetCanRewardedShowAd(true);
            }

        }
        public void HandleInterStetialAdLoaded(object sender,EventArgs args){
            if(GameHandler.current != null){
                GameHandler.current.SetCanShowInterstetialAds(true);
            }
        }
        public void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args){
            RequestRewardedAd();
            GameHandler.current.SetCanRewardedShowAd(false);
            GameHandler.current.SetIsRewardedAdsPlaying(false);
            GameHandler.current.SetGameOver(false);
            if (!rewardedAd.IsLoaded()){
                RequestRewardedAd();
            }
        }
        public void HandleInterStetailAdFailedToLoad(object sender, AdFailedToLoadEventArgs args){
            GameHandler.current.SetCanShowInterstetialAds(false);
            if(!interstitialAd.IsLoaded()){
                RequestInterstitial();
            }
        }

        public void HandleRewardedAdOpening(object sender, EventArgs args){
            // AudioManager.i.PauseMusic(SoundType.Main_Sound);
        }
        public void HandleInterstitialAdAdOpening(object sender, EventArgs args){
            // AudioManager.i.PauseMusic(SoundType.Main_Sound);
        }

        public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args){
            if(!rewardedAd.IsLoaded()){
                RequestRewardedAd();
            }
            
            GameHandler.current.SetIsRewardedAdsPlaying(false);
            GameHandler.current.SetGameOver(false);
        }
        public void HandleinterstitialAdAdFailedToShow(object sender,AdErrorEventArgs args){
            if(!interstitialAd.IsLoaded()){
                RequestInterstitial();
            }
            GameHandler.current.SetCanShowInterstetialAds(false);
            
        }

        public void HandleRewardedAdClosed(object sender, EventArgs args){
            if (!rewardedAd.IsLoaded()){
                RequestRewardedAd();
            }
            
        }
        private void HandleUserEarnedReward(object sender, Reward args){
            GameHandler.current.RevivePlayer();
            GameHandler.current.SetIsRewardedAdsPlaying(false);
            GameHandler.current.SetCanRewardedShowAd(false);
            RequestRewardedAd();
            
        }

        #endregion
        
        
        
    }
}
