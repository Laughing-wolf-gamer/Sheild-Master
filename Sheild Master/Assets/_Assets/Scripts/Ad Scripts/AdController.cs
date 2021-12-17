using System;
using UnityEngine;
// using GoogleMobileAds.Api;
using Yodo1.MAS;

namespace SheildMaster{

    public class AdController : MonoBehaviour{

        // private readonly string interstitialId = "ca-app-pub-3940256099942544/1033173712";
        // private readonly string rewardedId = "ca-app-pub-3940256099942544/5224354917";
        // private readonly string bannerId = "ca-app-pub-3940256099942544/6300978111";
        // private readonly string appId = "ca-app-pub-1447736674902262~2052801395";

        // private RewardedAd rewardedAd;
        // private InterstitialAd interstitialAd;
        // private BannerView bannerView;

        // public int HandleOnAdLeavingApplication { get; private set; }

        // private int npa;

        public bool askingforExtraCoinFromShop;
        public bool askinforExtraCoinFromGame;
        public bool isRewardedAdLoaded;
        public bool isInterstialLoaded;
        public static AdController current;

        private void Awake(){
            if (current == null){
                current = this;
            }
            else{
                Destroy(current.gameObject);
            }
            DontDestroyOnLoad(current.gameObject);
            
            // MobileAds.Initialize(initStatus => {
            //     Debug.Log("Init Stauts " + initStatus);
            // });
            
        }


        
        private void Start(){
            // InitializeAd();
            Yodo1U3dMas.InitializeSdk();
            SetInterStetialAdsCallBack();
            SetRewardAdsCallBack();
            
        }
        private void Update(){
            // if(IsRewardedAdsLoaded()){
            //     if(askinforExtraCoinFromGame){
            //         if(GameHandler.current != null){
            //             GameHandler.current.SetCanRewardedShowAd(true);
            //         }
            //     }
            // }
            isRewardedAdLoaded = IsRewardedAdsLoaded();
            isInterstialLoaded = isInterstialAdsLoaded();
            // else{

            //     SetRewardAdsCallBack();
            //     if(askinforExtraCoinFromGame){
            //         GameHandler.current.SetCanRewardedShowAd(false);
            //         GameHandler.current.SetIsRewardedAdsPlaying(false);
            //     }
            //     if (!IsRewardedAdsLoaded()){
            //         SetRewardAdsCallBack();
            //     }
            // }
        }
        private void SetInterStetialAdsCallBack(){
            
            Yodo1U3dMasCallback.Interstitial.OnAdOpenedEvent += OnInterstitialAdOpenedEvent;
            Yodo1U3dMasCallback.Interstitial.OnAdClosedEvent += OnInterstitialAdClosedEvent;
            Yodo1U3dMasCallback.Interstitial.OnAdErrorEvent += OnInterstitialAdErorEvent;
        }

        private void OnInterstitialAdErorEvent(Yodo1U3dAdError error) {

            if(!isInterstialAdsLoaded()){
                SetInterStetialAdsCallBack();
            }
            AudioManager.current.PauseMusic(SoundType.BGM);
        }

        private void OnInterstitialAdClosedEvent(){
            SetInterStetialAdsCallBack();
            AudioManager.current.PlayMusic(SoundType.BGM);
        }

        private void OnInterstitialAdOpenedEvent(){
            AudioManager.current.PauseMusic(SoundType.BGM);
        }
        public bool isInterstialAdsLoaded(){
            return Yodo1U3dMas.IsInterstitialAdLoaded();
        }

        public void ShowInterStaialAds(){
            if(isInterstialAdsLoaded()){
                Yodo1U3dMas.ShowInterstitialAd();
            }else{
                SetInterStetialAdsCallBack();
                if (isInterstialAdsLoaded()){
                    Yodo1U3dMas.ShowInterstitialAd();
                }
                else {
                    Debug.Log("Interstitial is not loaded");
                    SetInterStetialAdsCallBack();
                }
            }
        }

        private void SetRewardAdsCallBack(){
            
            Yodo1U3dMasCallback.Rewarded.OnAdOpenedEvent += OnRewardedAdOpenedEvent;
            Yodo1U3dMasCallback.Rewarded.OnAdClosedEvent += OnRewardedAdClosedEvent;
            Yodo1U3dMasCallback.Rewarded.OnAdReceivedRewardEvent += OnAdReceivedRewardEvent;
            Yodo1U3dMasCallback.Rewarded.OnAdErrorEvent += OnRewardedAdErorEvent;
        }

        private void OnRewardedAdErorEvent(Yodo1U3dAdError error){
            SetRewardAdsCallBack();
            if(askinforExtraCoinFromGame){
                GameHandler.current.SetCanRewardedShowAd(false);
                GameHandler.current.SetIsRewardedAdsPlaying(false);
            }
            if(askingforExtraCoinFromShop){
                AskAdForCoin.askAdForCoinCurrent.RewardCoinWithCoins(false);
            }
            if (!IsRewardedAdsLoaded()){
                SetRewardAdsCallBack();
            }
        }

        private void OnAdReceivedRewardEvent(){
            if(askinforExtraCoinFromGame){
                GameHandler.current.GivePlayerTwiceCash();
                GameHandler.current.SetIsRewardedAdsPlaying(false);
                GameHandler.current.SetCanRewardedShowAd(false);
            }
            if(askingforExtraCoinFromShop){
                AskAdForCoin.askAdForCoinCurrent.RewardCoinWithCoins(true);
                
            }

            SetRewardAdsCallBack();
        }

        private void OnRewardedAdClosedEvent(){
            AudioManager.current.PlayMusic(SoundType.BGM);
            if (!IsRewardedAdsLoaded()){
                SetRewardAdsCallBack();
            }

            AnayltyicsManager.current.OnAdcompleteAnayltyics_UnityAnayltics(true,UnityEngine.Analytics.AdvertisingNetwork.AdMob);
        }

        private void OnRewardedAdOpenedEvent(){
            AudioManager.current.PauseMusic(SoundType.BGM);
        }
        public bool IsRewardedAdsLoaded(){
            return Yodo1U3dMas.IsRewardedAdLoaded();
        }
        public void ShowRewarededAds(){
            if(IsRewardedAdsLoaded()){
                Yodo1U3dMas.ShowRewardedAd();
                if(askinforExtraCoinFromGame){
                    GameHandler.current.SetIsRewardedAdsPlaying(true);
                }
                AnayltyicsManager.current.SetAdImpressionDataAnayltyics();
            }else{
                SetRewardAdsCallBack();
                if(IsRewardedAdsLoaded()){
                    Yodo1U3dMas.ShowRewardedAd();
                    GameHandler.current.SetIsRewardedAdsPlaying(true);
                    AnayltyicsManager.current.SetAdImpressionDataAnayltyics();
                }else{
                    GameHandler.current.SetIsRewardedAdsPlaying(false);
                }
            }
        }

        // private void InitializeAd(){

        //     // // Interstestial Ads...
        //     // interstitialAd?.Destroy();
        //     // interstitialAd = new InterstitialAd(interstitialId);
        //     // interstitialAd.OnAdClosed += (sender, args) => {
        //     //     RequestInterstitial();
        //     //     AnayltyicsManager.current.OnAdcompleteAnayltyics_UnityAnayltics(false,UnityEngine.Analytics.AdvertisingNetwork.AdMob);
        //     // };
        //     // interstitialAd.OnAdFailedToLoad += HandleInterStetailAdFailedToLoad;
        //     // interstitialAd.OnAdFailedToShow += HandleinterstitialAdAdFailedToShow;
        //     // interstitialAd.OnAdLoaded += HandleInterStetialAdLoaded;
        //     // interstitialAd.OnAdOpening += HandleInterstitialAdAdOpening;

        //     // RequestInterstitial();

        //     // // Rewarded Ad.
        //     // rewardedAd = new RewardedAd(rewardedId);
        //     // rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        //     // rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        //     // rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        //     // rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        //     // rewardedAd.OnAdClosed += HandleRewardedAdClosed;
        //     // RequestRewardedAd();
            

        //     // Banner Ad.
        //     // bannerView?.Destroy();

        //     // bannerView = new BannerView(bannerId,AdSize.SmartBanner,AdPosition.BottomRight);
        //     // // Called When an ad is Closed.
        //     // bannerView.OnAdClosed += (object sender, EventArgs e) => RequestBannerAd();
        //     // // Called when an ad request has successfully loaded.
        //     // bannerView.OnAdLoaded += HandleOnAdLoaded;
        //     // // Called when an ad request failed to load.
        //     // bannerView.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        //     // RequestBannerAd();

        // }


        

        

        



        #region Rewarded Ad Handle

        // private void RequestRewardedAd(){
        //     AdRequest request = new AdRequest.Builder().Build();
        //     rewardedAd.LoadAd(request);
        // }
        // public bool IsRewardedAdLoaded(){
        //     return rewardedAd.IsLoaded();
        // }

        // public void ShowRewardedAd(){
        //     // if (rewardedAd.IsLoaded()){
        //     //     rewardedAd.Show();
        //     //     GameHandler.current.SetIsRewardedAdsPlaying(true);
        //     //     AnayltyicsManager.current.SetAdImpressionDataAnayltyics();
        //     // }else{
        //     //     RequestRewardedAd();
        //     //     if (rewardedAd.IsLoaded()){
        //     //         rewardedAd.Show();
        //     //         GameHandler.current.SetIsRewardedAdsPlaying(true);
        //     //     }
        //     //     else{
        //     //         GameHandler.current.SetIsRewardedAdsPlaying(false);
        //     //     }
        //     // }
        // }

        // public void HandleRewardedAdLoaded(object sender, EventArgs args){
        //     if(GameHandler.current != null){
        //         GameHandler.current.SetCanRewardedShowAd(true);
        //     }

        // }
        
        // public void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args){
        //     RequestRewardedAd();
        //     GameHandler.current.SetCanRewardedShowAd(false);
        //     GameHandler.current.SetIsRewardedAdsPlaying(false);
        //     if (!rewardedAd.IsLoaded()){
        //         RequestRewardedAd();
        //     }
        // }

        // public void HandleRewardedAdOpening(object sender, EventArgs args){
        //     // AudioManager.i.PauseMusic(SoundType.Main_Sound);
        //     AnayltyicsManager.current.SetAdImpressionDataAnayltyics();
        // }
        

        // public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args){
        //     if(!rewardedAd.IsLoaded()){
        //         RequestRewardedAd();
        //     }
            
        //     GameHandler.current.SetIsRewardedAdsPlaying(false);
        // }
        // public void HandleRewardedAdClosed(object sender, EventArgs args){
        //     if (!rewardedAd.IsLoaded()){
        //         RequestRewardedAd();
        //     }
        //     AnayltyicsManager.current.OnAdcompleteAnayltyics_UnityAnayltics(true,UnityEngine.Analytics.AdvertisingNetwork.AdMob);
        // }
        // private void HandleUserEarnedReward(object sender, Reward args){
        //     GameHandler.current.GivePlayerTwiceCash();
        //     GameHandler.current.SetIsRewardedAdsPlaying(false);
        //     GameHandler.current.SetCanRewardedShowAd(false);
        //     RequestRewardedAd();
            
        // }

        #endregion

        #region InterStatetialAd.
        // private void RequestInterstitial(){
        //     AdRequest request = new AdRequest.Builder().Build();
        //     interstitialAd.LoadAd(request);
        // }
        
        // public void ShowInterstitialAd(){
        //     // if (interstitialAd.IsLoaded()){
        //     //     interstitialAd.Show();
        //     // }
        //     // else{
        //     //     RequestInterstitial();
        //     //     if (interstitialAd.IsLoaded()){
        //     //         interstitialAd.Show();
        //     //     }
        //     //     else {
        //     //         Debug.Log("Interstitial is not loaded");
        //     //     }
        //     // }
        // }
        // public bool isInterstetialAdLoaded(){
        //     return interstitialAd.IsLoaded();
        // }
        // public void HandleInterStetailAdFailedToLoad(object sender, AdFailedToLoadEventArgs args){
        //     GameHandler.current.SetCanShowInterstetialAds(false);
        //     if(!interstitialAd.IsLoaded()){
        //         RequestInterstitial();
        //     }
        // }
        // public void HandleinterstitialAdAdFailedToShow(object sender,AdErrorEventArgs args){
        //     if(!interstitialAd.IsLoaded()){
        //         RequestInterstitial();
        //     }
        //     GameHandler.current.SetCanShowInterstetialAds(false);
        // }
        // public void HandleInterStetialAdLoaded(object sender,EventArgs args){
        //     if(GameHandler.current != null){
        //         GameHandler.current.SetCanShowInterstetialAds(true);
        //     }
        // }
        // public void HandleInterstitialAdAdOpening(object sender, EventArgs args){
        //     // AudioManager.i.PauseMusic(SoundType.Main_Sound);
        //     AnayltyicsManager.current.SetAdImpressionDataAnayltyics();
        // }
        

        
        

        #endregion
        


        #region Banner AD...
        // private void RequestBannerAd(){
        //     AdRequest request = new AdRequest.Builder().Build();
            
        //     bannerView.LoadAd(request);
        // }
        // private void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs e){
        //     RequestBannerAd();
        // }

        // private void HandleOnAdLoaded(object sender, EventArgs e){
        //     bannerView.Show();
        // }


        #endregion
        
    }
}
