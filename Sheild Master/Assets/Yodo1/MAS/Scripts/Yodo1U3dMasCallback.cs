using UnityEngine;
using System.Collections.Generic;
using System;

namespace Yodo1.MAS
{
    public class Yodo1U3dMasCallback : MonoBehaviour
    {
        private const int FLAG_INITIALIZE = 0;
        private const int FLAG_AD_EVENT = 1;

        private const int EVENT_INITIALIZE_FAILURE = 0;
        private const int EVENT_INITIALIZE_SUCCESS = 1;

        static bool _initialized = false;

        public enum AdType
        {
            Rewarded = 1,
            Interstitial = 2,
            Banner = 3,
        }

        public static Yodo1U3dMasCallback Instance { get; private set; }

        public string SdkMethodName
        {
            get
            {
                return "Yodo1U3dMasCallbackResult";
            }
        }

        public string SdkObjectName
        {
            get
            {
                return gameObject.name;
            }
        }

        public static bool isInitialized()
        {
#if UNITY_EDITOR
            return true;
#else
            return _initialized;
#endif
        }

        private static Yodo1U3dMas.InitializeDelegate _initializeDelegate;
        public static void SetInitializeDelegate(Yodo1U3dMas.InitializeDelegate initializeDelegate)
        {
            _initializeDelegate = initializeDelegate;
        }

        public static Yodo1U3dMas.InitializeDelegate GetInitializeDelegate()
        {
            return _initializeDelegate;
        }

        #region Ad Delegate

        //InterstitialAd of delegate
        private static Yodo1U3dMas.InterstitialAdDelegate _interstitialAdDelegate;
        public static void SetInterstitialAdDelegate(Yodo1U3dMas.InterstitialAdDelegate interstitialAdDelegate)
        {
            _interstitialAdDelegate = interstitialAdDelegate;
        }

        //ShowBanner of delegate
        private static Yodo1U3dMas.BannerdAdDelegate _bannerDelegate;
        public static void SetBannerAdDelegate(Yodo1U3dMas.BannerdAdDelegate bannerDelegate)
        {
            _bannerDelegate = bannerDelegate;
        }

        //RewardVideo of delegate
        private static Yodo1U3dMas.RewardedAdDelegate _rewardedAdDelegate;
        public static void SetRewardedAdDelegate(Yodo1U3dMas.RewardedAdDelegate rewardedAdDelegate)
        {
            _rewardedAdDelegate = rewardedAdDelegate;
        }

        #endregion

        private static bool CanInvokeEvent(Delegate evt)
        {
            if (evt == null) return false;

            if (evt.GetInvocationList().Length > 5)
            {
            }

            return true;
        }

        private static void InvokeEvent(Action evt)
        {
            if (!CanInvokeEvent(evt)) return;

            evt();
        }

        public static void InvokeEvent<T>(Action<T> evt, T param)
        {
            if (!CanInvokeEvent(evt)) return;

            evt(param);
        }

        public static void InvokeEvent<T1, T2>(Action<T1, T2> evt, T1 param1, T2 param2)
        {
            if (!CanInvokeEvent(evt)) return;

            evt(param1, param2);
        }

        // Fired when the SDK has finished initializing
        private static Action<bool, Yodo1U3dAdError> _onSdkInitializedEvent;
        public static event Action<bool, Yodo1U3dAdError> OnSdkInitializedEvent
        {
            add
            {
                _onSdkInitializedEvent += value;
            }
            remove
            {
                _onSdkInitializedEvent -= value;
            }
        }

        private static Action _onBannerAdOpenedEvent;
        private static Action<Yodo1U3dAdError> _onBannerAdErrorEvent;
        private static Action _onBannerAdClosedEvent;

        public class Banner
        {
            public static event Action OnAdOpenedEvent
            {
                add
                {
                    _onBannerAdOpenedEvent += value;
                }
                remove
                {
                    _onBannerAdOpenedEvent -= value;
                }
            }

            public static event Action<Yodo1U3dAdError> OnAdErrorEvent
            {
                add
                {
                    _onBannerAdErrorEvent += value;
                }
                remove
                {
                    _onBannerAdErrorEvent -= value;
                }
            }

            public static event Action OnAdClosedEvent
            {
                add
                {
                    _onBannerAdClosedEvent += value;
                }
                remove
                {
                    _onBannerAdClosedEvent -= value;
                }
            }
        }

        private static Action _onInterstitialAdOpenedEvent;
        private static Action<Yodo1U3dAdError> _onInterstitialAdErrorEvent;
        private static Action _onInterstitialAdClosedEvent;

        public class Interstitial
        {
            /**
             * Fired when an interstitial ad is displayed (may not be received by Unity until the interstitial ad closes).
             */
            public static event Action OnAdOpenedEvent
            {
                add
                {
                    _onInterstitialAdOpenedEvent += value;
                }
                remove
                {
                    _onInterstitialAdOpenedEvent -= value;
                }
            }

            public static event Action<Yodo1U3dAdError> OnAdErrorEvent
            {
                add
                {
                    _onInterstitialAdErrorEvent += value;
                }
                remove
                {
                    _onInterstitialAdErrorEvent -= value;
                }
            }

            public static event Action OnAdClosedEvent
            {
                add
                {
                    _onInterstitialAdClosedEvent += value;
                }
                remove
                {
                    _onInterstitialAdClosedEvent -= value;
                }
            }
        }

        private static Action _onRewardedAdOpenedEvent;
        private static Action<Yodo1U3dAdError> _onRewardedAdErrorEvent;
        private static Action _onRewardedAdReceivedRewardEvent;
        private static Action _onRewardedAdClosedEvent;

        public class Rewarded
        {
            /**
             * Fired when an rewarded ad is displayed (may not be received by Unity until the rewarded ad closes).
             */
            public static event Action OnAdOpenedEvent
            {
                add
                {
                    _onRewardedAdOpenedEvent += value;
                }
                remove
                {
                    _onRewardedAdOpenedEvent -= value;
                }
            }

            public static event Action<Yodo1U3dAdError> OnAdErrorEvent
            {
                add
                {
                    _onRewardedAdErrorEvent += value;
                }
                remove
                {
                    _onRewardedAdErrorEvent -= value;
                }
            }

            public static event Action OnAdReceivedRewardEvent
            {
                add
                {
                    _onRewardedAdReceivedRewardEvent += value;
                }
                remove
                {
                    _onRewardedAdReceivedRewardEvent -= value;
                }
            }

            public static event Action OnAdClosedEvent
            {
                add
                {
                    _onRewardedAdClosedEvent += value;
                }
                remove
                {
                    _onRewardedAdClosedEvent -= value;
                }
            }
        }

        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        public void Yodo1U3dMasCallbackResult(string result)
        {
            Debug.Log(Yodo1U3dMas.TAG + "The SDK callback result:" + result + "\n");

            Dictionary<string, object> obj = (Dictionary<string, object>)Yodo1JSON.Deserialize(result);
            if (obj == null)
            {
                return;
            }

            if (!obj.ContainsKey("flag") || !obj.ContainsKey("data"))
            {
                return;
            }

            string jsonData = obj["data"].ToString();
            Dictionary<string, object> dataDic = (Dictionary<string, object>)Yodo1JSON.Deserialize(jsonData);
            if (dataDic == null)
            {
                return;
            }

            int flag = int.Parse(obj["flag"].ToString());
            if (flag == FLAG_INITIALIZE)
            {
                bool success = false;
                Yodo1U3dAdError error = null;

                if (dataDic.ContainsKey("success"))
                {
                    success = int.Parse(dataDic["success"].ToString()) == EVENT_INITIALIZE_SUCCESS ? true : false;
                }
                if (dataDic.ContainsKey("error"))
                {
                    error = Yodo1U3dAdError.createWithJson(dataDic["error"].ToString());
                }
                else
                {
                    error = new Yodo1U3dAdError();
                }

                if (_initializeDelegate != null)
                {
                    _initializeDelegate(success, error);
                }

                InvokeEvent(_onSdkInitializedEvent, success, error);

                _initialized = true;
            }
            else if (flag == FLAG_AD_EVENT)
            {
                AdType type = AdType.Rewarded;
                if (dataDic.ContainsKey("type"))
                {
                    type = (AdType)int.Parse(dataDic["type"].ToString());
                }
                else
                {
                    return;
                }
                Yodo1U3dAdEvent adEvent = Yodo1U3dAdEvent.AdError;
                if (dataDic.ContainsKey("code"))
                {
                    adEvent = (Yodo1U3dAdEvent)int.Parse(dataDic["code"].ToString());
                }
                string message;
                if (dataDic.ContainsKey("message"))
                {
                    message = dataDic["message"].ToString();
                }
                Yodo1U3dAdError adError = null;
                if (dataDic.ContainsKey("error"))
                {
                    adError = Yodo1U3dAdError.createWithJson(Yodo1JSON.Serialize(dataDic["error"]));
                }
                else
                {
                    adError = new Yodo1U3dAdError();
                }

                string indexId = "";
                if (dataDic.ContainsKey("indexId"))
                {
                    indexId = dataDic["indexId"].ToString();
                }

                switch (type)
                {
                    case AdType.Rewarded:
                        {
                            if (_rewardedAdDelegate != null)
                            {
                                _rewardedAdDelegate(adEvent, adError);
                            }

                            RewardedCallbacksEvent(adEvent, adError);
                        }
                        break;
                    case AdType.Interstitial:
                        {
                            if (_interstitialAdDelegate != null)
                            {
                                _interstitialAdDelegate(adEvent, adError);
                            }

                            InterstitialCallbacksEvent(adEvent, adError);
                        }
                        break;
                    case AdType.Banner:
                        {
                            if (_bannerDelegate != null)
                            {
                                _bannerDelegate(adEvent, adError);
                            }

                            BannerCallbacksEvent(adEvent, adError);

                            Yodo1U3dBannerAdView.CallbcksEvent(adEvent, adError, indexId);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private void BannerCallbacksEvent(Yodo1U3dAdEvent adEvent, Yodo1U3dAdError adError)
        {
            switch (adEvent)
            {
                case Yodo1U3dAdEvent.AdError:
                    InvokeEvent(_onBannerAdErrorEvent, adError);
                    break;
                case Yodo1U3dAdEvent.AdOpened:
                    InvokeEvent(_onBannerAdOpenedEvent);
                    break;
                case Yodo1U3dAdEvent.AdClosed:
                    InvokeEvent(_onBannerAdClosedEvent);
                    break;
            }
        }

        private void InterstitialCallbacksEvent(Yodo1U3dAdEvent adEvent, Yodo1U3dAdError adError)
        {
            switch (adEvent)
            {
                case Yodo1U3dAdEvent.AdError:
                    InvokeEvent(_onInterstitialAdErrorEvent, adError);
                    break;
                case Yodo1U3dAdEvent.AdOpened:
                    InvokeEvent(_onInterstitialAdOpenedEvent);
                    break;
                case Yodo1U3dAdEvent.AdClosed:
                    InvokeEvent(_onInterstitialAdClosedEvent);
                    break;
            }
        }

        private void RewardedCallbacksEvent(Yodo1U3dAdEvent adEvent, Yodo1U3dAdError adError)
        {
            switch (adEvent)
            {
                case Yodo1U3dAdEvent.AdError:
                    InvokeEvent(_onRewardedAdErrorEvent, adError);
                    break;
                case Yodo1U3dAdEvent.AdOpened:
                    InvokeEvent(_onRewardedAdOpenedEvent);
                    break;
                case Yodo1U3dAdEvent.AdReward:
                    InvokeEvent(_onRewardedAdReceivedRewardEvent);
                    break;
                case Yodo1U3dAdEvent.AdClosed:
                    InvokeEvent(_onRewardedAdClosedEvent);
                    break;
            }
        }
    }
}