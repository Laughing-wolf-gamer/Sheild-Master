using System;
using UnityEngine;
using SheildMaster;
using System.Collections.Generic;

public class AdManager : MonoBehaviour {
    public static AdManager instance;
    public static int adCount = 0;
    // public int showGameWonInterstitalAdAfter;
    public string ironsourceAppKey;

    public static bool dailyReward = false, skinShopTry = false, winDouble = false, watchAdCoin = false,watchAdDimond = false;
    private readonly string admobKey = "ca-app-pub-1447736674902262~5709686781";
    public bool IsRewardedAdsLoaded{get;private set;}
    private void Awake(){
        if(instance == null){
            instance = this;
        }else{
            Destroy(instance.gameObject);
        }
        DontDestroyOnLoad(instance.gameObject);
    }

    private void Start(){
        // Initialize the Ironsource Ads SDK.
        IronSource.Agent.init(ironsourceAppKey);
        IronSource.Agent.validateIntegration();
        IronSource.Agent.shouldTrackNetworkState(true);
        RequestInterstitial();
        RequestRewarded();
    }

    public void RequestBanner(){
        IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, IronSourceBannerPosition.BOTTOM);        
    }

    private void RequestInterstitial(){
        IronSourceEvents.onInterstitialAdReadyEvent += InterstitialAdReadyEvent;
        IronSourceEvents.onInterstitialAdLoadFailedEvent += InterstitialAdLoadFailedEvent;
        IronSourceEvents.onInterstitialAdShowSucceededEvent += InterstitialAdShowSucceededEvent;
        IronSourceEvents.onInterstitialAdShowFailedEvent += InterstitialAdShowFailedEvent;
        IronSourceEvents.onInterstitialAdClickedEvent += InterstitialAdClickedEvent;
        IronSourceEvents.onInterstitialAdOpenedEvent += InterstitialAdOpenedEvent;
        IronSourceEvents.onInterstitialAdClosedEvent += InterstitialAdClosedEvent;
        IronSourceEvents.onImpressionDataReadyEvent += OnImpressionReadyEvent;
        IronSourceEvents.onImpressionSuccessEvent += OnImpressionSuccesseEvent;

        IronSource.Agent.loadInterstitial();
    }


    private void RequestRewarded(){
        IronSourceEvents.onRewardedVideoAdOpenedEvent += RewardedVideoAdOpenedEvent;
        IronSourceEvents.onRewardedVideoAdClickedEvent += RewardedVideoAdClickedEvent;
        IronSourceEvents.onRewardedVideoAdClosedEvent += RewardedVideoAdClosedEvent;
        IronSourceEvents.onRewardedVideoAvailabilityChangedEvent += RewardedVideoAvailabilityChangedEvent;
        IronSourceEvents.onRewardedVideoAdStartedEvent += RewardedVideoAdStartedEvent;
        IronSourceEvents.onRewardedVideoAdEndedEvent += RewardedVideoAdEndedEvent;
        IronSourceEvents.onRewardedVideoAdRewardedEvent += RewardedVideoAdRewardedEvent;
        IronSourceEvents.onRewardedVideoAdShowFailedEvent += RewardedVideoAdShowFailedEvent;
        IronSourceEvents.onImpressionDataReadyEvent += OnImpressionReadyEvent;
        IronSourceEvents.onImpressionSuccessEvent += OnImpressionSuccesseEvent;
        
    }

    #region Interstitial Ads Handlers

    // Invoked when the initialization process has failed.
    // @param description - string - contains information about the failure.
    private void InterstitialAdLoadFailedEvent(IronSourceError error){
        
    }
    // Invoked when the ad fails to show.
    // @param description - string - contains information about the failure.
    private void InterstitialAdShowFailedEvent(IronSourceError error){
        
    }
    // Invoked when end user clicked on the interstitial ad
    private void InterstitialAdClickedEvent(){

    }
    // Invoked when the interstitial ad closed and the user goes back to the application screen.
    private void InterstitialAdClosedEvent(){
        RequestInterstitial();
    }
    // Invoked when the Interstitial is Ready to shown after load function is called
    private void InterstitialAdReadyEvent(){
        // IsRewardedAdsLoaded = true;
    }
    // Invoked when the Interstitial Ad Unit has opened
    private void InterstitialAdOpenedEvent(){
        AnayltyicsManager.current.SetIntersteailAdsData();
    }
    // Invoked right before the Interstitial screen is about to open.
    // NOTE - This event is available only for some of the networks. 
    // You should treat this event as an interstitial impression, but rather use InterstitialAdOpenedEvent
    private void InterstitialAdShowSucceededEvent(){
        AnayltyicsManager.current.OnAdcompleteAnayltyics_UnityAnayltics(false,UnityEngine.Analytics.AdvertisingNetwork.IronSource);
    }
    private void OnImpressionReadyEvent(IronSourceImpressionData impressionData){
        AnayltyicsManager.current.ImpressionSuccesessEvent(impressionData);
    }
    private void OnImpressionSuccesseEvent(IronSourceImpressionData impressionData){
        AnayltyicsManager.current.ImpressionSuccesessEvent(impressionData);
    }

    #endregion

    #region Reward Ads Handlers

    //Invoked when the RewardedVideo ad view has opened.
    //Your Activity will lose focus. Please avoid performing heavy 
    //tasks till the video ad will be closed.
    private void RewardedVideoAdOpenedEvent(){

    }
    //Invoked when the RewardedVideo ad view is about to be closed.
    //Your activity will now regain its focus.
    private void RewardedVideoAdClosedEvent(){
        RequestRewarded();
        AnayltyicsManager.current.OnAdcompleteAnayltyics_UnityAnayltics(true,UnityEngine.Analytics.AdvertisingNetwork.IronSource);
        if(winDouble){
            UIHandler.current.Show_hideRewardAdsButton(false);
        }
    }
    //Invoked when there is a change in the ad availability status.
    //@param - available - value will change to true when rewarded videos are available. 
    //You can then show the video by calling showRewardedVideo().
    //Value will change to false when no videos are available.
    private void RewardedVideoAvailabilityChangedEvent(bool available){
        //Change the in-app 'Traffic Driver' state according to availability.
        IsRewardedAdsLoaded = available;
    }

    //Invoked when the user completed the video and should be rewarded. 
    //If using server-to-server callbacks you may ignore this events and wait for 
    // the callback from the  ironSource server.
    //@param - placement - placement object which contains the reward data
    private void RewardedVideoAdRewardedEvent(IronSourcePlacement placement){
        if (dailyReward){
            // Here you have to call the Claim2X() from GameEventManager.
            GameEventManager.eventManager.Claim5X();
            dailyReward = false;
        }else if (skinShopTry){
            SkinShopHandler.current.TryCurrentSkinAfterAd();
            skinShopTry = false;
        }else if (winDouble){
            LevelManager.current.AddTwiceMoney();
            UIHandler.current.Show_hideRewardAdsButton(false);
            winDouble = false;
        }else if (watchAdCoin){
            AskAdForCoin.current.RewardCoin();
            watchAdCoin = false;
        }else if (watchAdDimond){
            AskAdForDimond.current.RewardCoin();
            watchAdDimond = false;
        }
    }
    //Invoked when the Rewarded Video failed to show
    //@param description - string - contains information about the failure.
    private void RewardedVideoAdShowFailedEvent(IronSourceError error){
        dailyReward = false;
        skinShopTry = false;
        winDouble = false;
        watchAdCoin = false;
        watchAdDimond = false;
        if(UIHandler.current != null){
            UIHandler.current.Show_hideRewardAdsButton(true);
        }
    }

    // ----------------------------------------------------------------------------------------
    // Note: the events below are not available for all supported rewarded video ad networks. 
    // Check which events are available per ad network you choose to include in your build. 
    // We recommend only using events which register to ALL ad networks you include in your build. 
    // ----------------------------------------------------------------------------------------

    //Invoked when the video ad starts playing. 
    private void RewardedVideoAdStartedEvent(){
        AnayltyicsManager.current.SetRewardAdsData();
    }
    //Invoked when the video ad finishes playing. 
    private void RewardedVideoAdEndedEvent(){
        IsRewardedAdsLoaded = false;
        
    }
    //Invoked when the video ad is clicked. 
    private void RewardedVideoAdClickedEvent(IronSourcePlacement placement){
    }



    #endregion

    public void GameOver(){
        if (IronSource.Agent.isInterstitialReady()){
            IronSource.Agent.showInterstitial();
            AnayltyicsManager.current.SetAdImpressionDataAnayltyics();
            AnayltyicsManager.current.SetIntersteailAdsData();
        }else {
            RequestInterstitial();
        }
    }

    // public void GameWon(){
    //     Debug.Log("Adcount is:- " + adCount);

    //     if(adCount >= showGameWonInterstitalAdAfter){
    //         if (this.interstitial.IsLoaded()){
    //             this.interstitial.Show();
    //             adCount = 0;
    //         }
    //     }
    //     else{
    //         adCount += 1;
    //     }
    // }

    public void UserChoseToWatchAd(){
        if (IronSource.Agent.isRewardedVideoAvailable()){
            IronSource.Agent.showRewardedVideo();
            AnayltyicsManager.current.SetAdImpressionDataAnayltyics();
            AnayltyicsManager.current.SetIntersteailAdsData();
        }
    }

    public void DestroyBanner(){
        IronSource.Agent.destroyBanner();
    }

    private void OnApplicationPause(bool isPaused){
        IronSource.Agent.onApplicationPause(isPaused);
    }
}
