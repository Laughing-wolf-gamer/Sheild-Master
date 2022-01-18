using System;
using UnityEngine;
using SheildMaster;
using GoogleMobileAds.Api;
using System.Collections.Generic;
using GoogleMobileAds.Api.Mediation.UnityAds;
using GoogleMobileAds.Api.Mediation.AdColony;

public class AdManager : MonoBehaviour {
    public static AdManager instance;
    private bool initAd = false;
    public static int adCount = 0;
    public int showGameWonInterstitalAdAfter;
    private BannerView bannerView;
    private InterstitialAd interstitial;
    private RewardedAd rewardedAd;
    public string bannerIdAndroid, bannerIdIos, interstitialIdAndroid, interstitialIdIos, rewardIdAndroid, rewardIdIos;

    public static bool dailyReward = false, skinShopTry = false, winDouble = false, watchAdCoin = false,watchAdDimond = false;
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
        // Initialize the Mobile Ads SDK.
        MobileAds.Initialize((initStatus) =>{
            Dictionary<string, AdapterStatus> map = initStatus.getAdapterStatusMap();
            foreach (KeyValuePair<string, AdapterStatus> keyValuePair in map){
                string className = keyValuePair.Key;
                AdapterStatus status = keyValuePair.Value;
                switch (status.InitializationState){
                    case AdapterState.NotReady:
                        // The adapter initialization did not complete.
                        Debug.Log("Adapter: " + className + " not ready.");
                        break;
                    case AdapterState.Ready:
                        // The adapter was successfully initialized.
                        MonoBehaviour.print("Adapter: " + className + " is initialized.");
                        initAd = true;
                        UnityAds.SetGDPRConsentMetaData(true);
                        AdColonyAppOptions.SetGDPRRequired(true);
                        AdColonyAppOptions.SetGDPRConsentString("1");
                        this.RequestInterstitial();
                        this.RequestRewarded();
                        break;
                }
            }
        });
    }

    public void RequestBanner(){
        if (initAd){
#if UNITY_ANDROID
            string adUnitId = bannerIdAndroid;
#elif UNITY_IPHONE
            string adUnitId = bannerIdIos;
#else
            string adUnitId = "unexpected_platform";
#endif

            // Create a 320x50 banner at the top of the screen.
            this.bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);

            // Create an empty ad request.
            AdRequest request = new AdRequest.Builder().Build();

            // Load the banner with the request.
            this.bannerView.LoadAd(request);
        }
    }

    private void RequestInterstitial(){
        if (initAd){
#if UNITY_ANDROID
            string adUnitId = interstitialIdAndroid;
#elif UNITY_IPHONE
        string adUnitId = interstitialIdIos;
#else
        string adUnitId = "unexpected_platform";
#endif

            // Initialize an InterstitialAd.
            this.interstitial = new InterstitialAd(adUnitId);

            // Called when an ad request has successfully loaded.
            this.interstitial.OnAdLoaded += HandleOnAdLoaded;
            // Called when an ad request failed to load.
            this.interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
            // Called when an ad is shown.
            this.interstitial.OnAdOpening += HandleOnAdOpened;
            // Called when the ad is closed.
            this.interstitial.OnAdClosed += HandleOnAdClosed;

            // Create an empty ad request.
            AdRequest request = new AdRequest.Builder().Build();
            // Load the interstitial with the request.
            this.interstitial.LoadAd(request);
        }
    }


    private void RequestRewarded(){
        string adUnitId;
#if UNITY_ANDROID
        adUnitId = rewardIdAndroid;
#elif UNITY_IPHONE
            adUnitId = rewardIdIos;
#else
            adUnitId = "unexpected_platform";
#endif

        this.rewardedAd = new RewardedAd(adUnitId);

        // Called when an ad request has successfully loaded.
        this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        // Called when an ad request failed to load.
        this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        // Called when an ad is shown.
        this.rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        // Called when an ad request failed to show.
        this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        // Called when the user should be rewarded for interacting with the ad.
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // Called when the ad is closed.
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(request);
    }

    #region Interstitial Ads Handlers

    public void HandleOnAdLoaded(object sender, EventArgs args){
        IsRewardedAdsLoaded = true;
        Debug.Log("HandleAdLoaded event received");
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args){
        IsRewardedAdsLoaded = false;
        Debug.Log("HandleFailedToReceiveAd event received with message: "
                            + args.ToString());
    }

    public void HandleOnAdOpened(object sender, EventArgs args){
        Debug.Log("HandleAdOpened event received");
    }

    public void HandleOnAdClosed(object sender, EventArgs args){
        Debug.Log("HandleAdClosed event received");
        interstitial.Destroy();
        this.RequestInterstitial();
    }

    #endregion

    #region Reward Ads Handlers

    public void HandleRewardedAdLoaded(object sender, EventArgs args){
        Debug.Log("HandleRewardedAdLoaded event received");
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args){
        Debug.Log("HandleRewardedAdFailedToLoad event received with message: "+ args.ToString());
        dailyReward = false;
        skinShopTry = false;
        winDouble = false;
        watchAdCoin = false;
        watchAdDimond = false;
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args){
        Debug.Log("HandleRewardedAdOpening event received");
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args){
        Debug.Log("HandleRewardedAdFailedToShow event received with message: "+ args.Message);
        dailyReward = false;
        skinShopTry = false;
        winDouble = false;
        watchAdCoin = false;
        watchAdDimond = false;
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args){
        Debug.Log("HandleRewardedAdClosed event received");
        this.RequestRewarded();
        
    }

    public void HandleUserEarnedReward(object sender, Reward args){
        if (dailyReward){
            // Here you have to call the Claim2X() from GameEventManager.
            GameEventManager.eventManager.Claim2X();
            dailyReward = false;
        }else if (skinShopTry){
            SkinShopHandler.current.TryCurrentSkinAfterAd();
            skinShopTry = false;
        }else if (winDouble){
            LevelManager.current.AddTwiceMoney();
            winDouble = false;
        }else if (watchAdCoin){
            AskAdForCoin.current.RewardCoin();
            watchAdCoin = false;
        }else if(watchAdDimond){
            AskAdForDimond.current.RewardCoin();
            watchAdDimond = false;
        }
    }

    #endregion

    public void GameOver(){
        if (this.interstitial.IsLoaded()){
            this.interstitial.Show();
        }
    }

    public void GameWon(){
        Debug.Log("Adcount is:- " + adCount);

        if(adCount >= showGameWonInterstitalAdAfter){
            if (this.interstitial.IsLoaded()){
                this.interstitial.Show();
                adCount = 0;
            }
        }
        else{
            adCount += 1;
        }
    }

    public void UserChoseToWatchAd(){
        if (this.rewardedAd.IsLoaded()){
            this.rewardedAd.Show();
        }
    }

    public void DestroyBanner(){
        if (bannerView != null){
            bannerView.Destroy();
        }
    }
}
