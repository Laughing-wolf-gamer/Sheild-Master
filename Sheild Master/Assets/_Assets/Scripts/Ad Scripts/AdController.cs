using UnityEngine;
using Yodo1.MAS;


namespace SheildMaster{

    public class AdController : MonoBehaviour {
        
        public bool askingforExtraCoinFromShop;
        public bool trySkinAds;
        public bool askinforExtraCoinFromGame;
        public static AdController current;

        

        private Yodo1U3dBannerAdView bannerAdsView;
        public bool rewardAdsAvailabe {get;private set;}
        private void Awake(){
            if (current == null){
                current = this;
            }
            else{
                Destroy(current.gameObject);
            }
            DontDestroyOnLoad(current.gameObject);
        }
        private void Start() {
            InitializeADs();
        } 

        public void InitializeADs(){
            
            Yodo1U3dMas.InitializeSdk();
            SetInterStetialAdsCallBack();
            SetRewardAdsCallBack();
        }

        #region Rewarded Ad Handle............

        public void SetRewardAdsCallBack(){
            
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
            if (!IsRewardedAdsLoaded()){
                SetRewardAdsCallBack();
            }
        }

        private void OnAdReceivedRewardEvent(){
            if(askinforExtraCoinFromGame){
                //reward player with extra coins after the gameplay...
                LevelManager.current.AddTwiceMoney();
                GameHandler.current.SetIsRewardedAdsPlaying(false);
                GameHandler.current.SetCanRewardedShowAd(false);
            }
            if(askingforExtraCoinFromShop){
                // for reward the player with extra coins in the coin shop...
                AskAdForCoin.current.RewardCoinWithCoins();
            }
            if(trySkinAds){
            //for try extra skin...
                SkinShopHandler.current.TryCurrentSkinAfterAd();
            }
        }

        private void OnRewardedAdClosedEvent(){
            if(askinforExtraCoinFromGame){
                AudioManager.current.PlayMusic(SoundType.BGM);
            }
            if (!IsRewardedAdsLoaded()){
                SetRewardAdsCallBack();
            }
            SetRewardAdsCallBack();
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

        

        #endregion

        #region InterStatetialAd.
        public void SetInterStetialAdsCallBack(){
            
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
            if(askingforExtraCoinFromShop){
                AudioManager.current.PlayMusic(SoundType.BGM);
            }
        }

        private void OnInterstitialAdOpenedEvent(){
            AudioManager.current.StopAudio(SoundType.BGM);
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
        

        
        

        #endregion
        


        #region Banner AD...
        public void RequestBannerView(){
            if(bannerAdsView != null){
                DestroyBannerView();
            }
            
            bannerAdsView = new Yodo1U3dBannerAdView(Yodo1U3dBannerAdSize.AdaptiveBanner,Yodo1U3dBannerAdPosition.BannerBottom);
            bannerAdsView.LoadAd();
        }
        public void DestroyBannerView(){
            bannerAdsView.Destroy();
            bannerAdsView = null;
        }


        #endregion


        // #region External Methods...

        // public bool GetExtraCoinFromShop(){
        //     return askinforExtraCoinFromGame;
        // }
        // public bool GetAskinforExtraCoinFromGame(){
        //     return askinforExtraCoinFromGame;
        // }
        // public bool GetTryGetSkinAd(){
        //     return trySkinAds;
        // }
        // public void AskingforExtraCoinFromShop(bool isActive){
        //     askingforExtraCoinFromShop = isActive;
        // }
        // public void AskinforExtraCoinFromGame(bool isActive){
        //     askinforExtraCoinFromGame = isActive;
        // }
        // public void SetTryGetSkinAd(bool isAcitve){
        //     trySkinAds = isAcitve;
        // }
        // #endregion
        
    }
}
