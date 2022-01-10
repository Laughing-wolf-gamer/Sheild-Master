using UnityEngine;
using System.Collections;
using Yodo1.MAS;

public class Yodo1AdsTest : MonoBehaviour
{
    bool enableBannerApiV2 = true;

    void Start()
    {
        Yodo1U3dMasCallback.OnSdkInitializedEvent += (success, error) =>
        {
            Debug.Log(Yodo1U3dMas.TAG + "OnSdkInitializedEvent, success:" + success + ", error: " + error.ToString());
            if (success)
            {
                InitializeBannerAds();
                InitializeInterstitialAds();
                InitializeRewardedAds();
            }
        };

        Yodo1AdBuildConfig config = new Yodo1AdBuildConfig()
            .enableAdaptiveBanner(true)
            .enableUserPrivacyDialog(true);
        Yodo1U3dMas.SetAdBuildConfig(config);

        Yodo1U3dMas.InitializeSdk();
    }

    void OnGUI()
    {
        int buttonHeight = Screen.height / 13;
        int buttonWidth = Screen.width / 2;
        int buttonSpace = buttonHeight / 2;
        int startHeight = buttonHeight / 2;

        if (GUI.Button(new Rect(Screen.width / 4, startHeight, buttonWidth, buttonHeight), "Show Banner Ad"))
        {
            ShowBannerAds();
        }

        if (GUI.Button(new Rect(Screen.width / 4, startHeight + buttonHeight + buttonSpace, buttonWidth, buttonHeight), "Dismiss Banner Ad"))
        {
            HideBannerAds();
        }

        if (GUI.Button(new Rect(Screen.width / 4, startHeight + buttonHeight * 2 + buttonSpace * 2, buttonWidth, buttonHeight), "Show Interstitial Ad"))
        {
            ShowInterstitialAds();
        }

        if (GUI.Button(new Rect(Screen.width / 4, startHeight + buttonHeight * 3 + buttonSpace * 3, buttonWidth, buttonHeight), "Show Rewarded Ad"))
        {
            ShowRewardedAds();
        }
    }

    #region Banner Ad Methods
    private void InitializeBannerAds()
    {
        if (enableBannerApiV2 == false)
        {
            InitializeBannerAdsV1();
        }
        else
        {
            InitializeBannerAdsV2();
        }
    }

    private void ShowBannerAds()
    {
        if (enableBannerApiV2 == false)
        {
            ShowBannerAdsV1();
        }
        else
        {
            ShowBannerAdsV2();
        }
    }

    private void HideBannerAds()
    {
        if (enableBannerApiV2 == false)
        {
            HideBannerAdsV1();
        }
        else
        {
            HideBannerAdsV2();
        }
    }
    #endregion

    #region Banner Ad Methods - V1
    private void InitializeBannerAdsV1()
    {
        Yodo1U3dMasCallback.Banner.OnAdOpenedEvent += OnBannerAdOpenedEvent;
        Yodo1U3dMasCallback.Banner.OnAdClosedEvent += OnBannerAdClosedEvent;
        Yodo1U3dMasCallback.Banner.OnAdErrorEvent += OnBannerAdErrorEvent;

        ShowBannerAdsV1();
    }

    private void OnBannerAdErrorEvent(Yodo1U3dAdError adError)
    {
        Debug.Log(Yodo1U3dMas.TAG + "Banner ad error - " + adError.ToString());
    }

    private void OnBannerAdOpenedEvent()
    {
        Debug.Log(Yodo1U3dMas.TAG + "Banner ad opened");
    }

    private void OnBannerAdClosedEvent()
    {
        Debug.Log(Yodo1U3dMas.TAG + "Banner ad closed");
    }

    private void ShowBannerAdsV1()
    {
        int align = Yodo1U3dBannerAlign.BannerTop | Yodo1U3dBannerAlign.BannerHorizontalCenter;
        Yodo1U3dMas.ShowBannerAd(align);
    }

    private void HideBannerAdsV1()
    {
        Yodo1U3dMas.DismissBannerAd();
    }
    #endregion

    #region Banner Ad Methods - V2
    Yodo1U3dBannerAdView bannerAdView = null;
    Yodo1U3dBannerAdView bannerAdView2 = null;

    /// <summary>
    /// The banner is displayed automatically after loaded
    /// </summary>
    private void InitializeBannerAdsV2()
    {
        // Clean up banner before reusing
        if (bannerAdView != null)
        {
            bannerAdView.Destroy();
            bannerAdView = null;
        }

        // Create a 320x50 banner at top of the screen
        bannerAdView = new Yodo1U3dBannerAdView(Yodo1U3dBannerAdSize.Banner, Yodo1U3dBannerAdPosition.BannerTop | Yodo1U3dBannerAdPosition.BannerHorizontalCenter);

        // Create a 320x50 banner ad at coordinate (0,50) on screen.
        //bannerAdView = new Yodo1U3dBannerAdView(Yodo1U3dBannerAdSize.Banner, 0, 50);

        // Add Events
        bannerAdView.OnAdLoadedEvent += OnBannerAdLoadedEvent;
        bannerAdView.OnAdFailedToLoadEvent += OnBannerAdFailedToLoadEvent;
        bannerAdView.OnAdOpenedEvent += OnBannerAdOpenedEvent;
        bannerAdView.OnAdFailedToOpenEvent += OnBannerAdFailedToOpenEvent;
        bannerAdView.OnAdClosedEvent += OnBannerAdClosedEvent;

        // Load banner ads, the banner ad will be displayed automatically after loaded
        bannerAdView.LoadAd();


        // Clean up banner before reusing
        if (bannerAdView2 != null)
        {
            bannerAdView2.Destroy();
            bannerAdView2 = null;
        }

        // Create a adaptive banner at top of the screen
        bannerAdView2 = new Yodo1U3dBannerAdView(Yodo1U3dBannerAdSize.AdaptiveBanner, Yodo1U3dBannerAdPosition.BannerBottom | Yodo1U3dBannerAdPosition.BannerHorizontalCenter);

        // Add Events
        bannerAdView2.OnAdLoadedEvent += OnBannerAdLoadedEvent;
        bannerAdView2.OnAdFailedToLoadEvent += OnBannerAdFailedToLoadEvent;
        bannerAdView2.OnAdOpenedEvent += OnBannerAdOpenedEvent;
        bannerAdView2.OnAdFailedToOpenEvent += OnBannerAdFailedToOpenEvent;
        bannerAdView2.OnAdClosedEvent += OnBannerAdClosedEvent;

        // Load banner ads, the banner ad will be displayed automatically after loaded
        bannerAdView2.LoadAd();
    }

    private void OnBannerAdLoadedEvent(Yodo1U3dBannerAdView adView)
    {
        // Banner ad is ready to be shown.
        Debug.Log(Yodo1U3dMas.TAG + "BannerV2 ad loaded");
    }

    private void OnBannerAdFailedToLoadEvent(Yodo1U3dBannerAdView adView, Yodo1U3dAdError adError)
    {
        Debug.Log(Yodo1U3dMas.TAG + "BannerV2 ad failed to load with error code: " + adError.ToString());
    }

    private void OnBannerAdOpenedEvent(Yodo1U3dBannerAdView adView)
    {
        Debug.Log(Yodo1U3dMas.TAG + "BannerV2 ad opened");
    }

    private void OnBannerAdFailedToOpenEvent(Yodo1U3dBannerAdView adView, Yodo1U3dAdError adError)
    {
        Debug.Log(Yodo1U3dMas.TAG + "BannerV2 ad failed to load with error code: " + adError.ToString());
    }

    private void OnBannerAdClosedEvent(Yodo1U3dBannerAdView adView)
    {
        Debug.Log(Yodo1U3dMas.TAG + "BannerV2 ad closed");
    }

    /// <summary>
    /// (Optional) Show banner ads
    /// </summary>
    private void ShowBannerAdsV2()
    {
        if (bannerAdView != null)
        {
            bannerAdView.Show();
        }
    }

    /// <summary>
    /// (Optional) Hide banner ads
    /// </summary>
    private void HideBannerAdsV2()
    {
        if (bannerAdView != null)
        {
            bannerAdView.Hide();
        }
    }
    #endregion

    #region Interstitial Ad Methods
    private void InitializeInterstitialAds()
    {
        Yodo1U3dMasCallback.Interstitial.OnAdOpenedEvent += OnInterstitialAdOpenedEvent;
        Yodo1U3dMasCallback.Interstitial.OnAdClosedEvent += OnInterstitialAdClosedEvent;
        Yodo1U3dMasCallback.Interstitial.OnAdErrorEvent += OnInterstitialAdErorEvent;
    }

    private void OnInterstitialAdOpenedEvent()
    {
        Debug.Log(Yodo1U3dMas.TAG + "Interstitial ad opened");
    }

    private void OnInterstitialAdClosedEvent()
    {
        Debug.Log(Yodo1U3dMas.TAG + "Interstitial ad closed");
    }

    private void OnInterstitialAdErorEvent(Yodo1U3dAdError adError)
    {
        Debug.Log(Yodo1U3dMas.TAG + "Interstitial ad error - " + adError.ToString());
    }

    private void ShowInterstitialAds()
    {
        if (Yodo1U3dMas.IsInterstitialAdLoaded())
        {
            Yodo1U3dMas.ShowInterstitialAd();
        }
        else
        {
            Debug.Log(Yodo1U3dMas.TAG + "Interstitial ad has not been cached.");
        }
    }
    #endregion

    #region Reward video Ad Methods
    private void InitializeRewardedAds()
    {
        Yodo1U3dMasCallback.Rewarded.OnAdOpenedEvent += OnRewardedAdOpenedEvent;
        Yodo1U3dMasCallback.Rewarded.OnAdClosedEvent += OnRewardedAdClosedEvent;
        Yodo1U3dMasCallback.Rewarded.OnAdReceivedRewardEvent += OnAdReceivedRewardEvent;
        Yodo1U3dMasCallback.Rewarded.OnAdErrorEvent += OnRewardedAdErorEvent;
    }

    private void OnRewardedAdOpenedEvent()
    {
        Debug.Log(Yodo1U3dMas.TAG + "Rewarded ad opened");
    }

    private void OnRewardedAdClosedEvent()
    {
        Debug.Log(Yodo1U3dMas.TAG + "Rewarded ad closed");
    }

    private void OnAdReceivedRewardEvent()
    {
        Debug.Log(Yodo1U3dMas.TAG + "Rewarded ad received reward");
    }

    private void OnRewardedAdErorEvent(Yodo1U3dAdError adError)
    {
        Debug.Log(Yodo1U3dMas.TAG + "Rewarded ad error - " + adError.ToString());
    }

    private void ShowRewardedAds()
    {
        if (Yodo1U3dMas.IsRewardedAdLoaded())
        {
            Yodo1U3dMas.ShowRewardedAd();
        }
        else
        {
            Debug.Log(Yodo1U3dMas.TAG + "Reward video ad has not been cached.");
        }
    }
    #endregion
}
