using System.Collections.Generic;

namespace Yodo1.MAS
{
    public class Yodo1AdBuildConfig
    {
        private bool _enableAdaptiveBanner;
        private bool _enableUserPrivacyDialog;
        private string _userAgreementUrl;
        private string _privacyPolicyUrl;

        /// <summary>
        /// Enable adaptive banner method, 
        /// <c>true</c>, if enable adaptive banner, <c>false</c> otherwise.
        /// </summary>
        /// <param name="adaptiveBanner"><c>true</c>, if enable adaptive banner, <c>false</c> otherwise.</param>
        /// <returns></returns>
        public Yodo1AdBuildConfig enableAdaptiveBanner(bool adaptiveBanner)
        {
            this._enableAdaptiveBanner = adaptiveBanner;
            return this;
        }

        /// <summary>
        /// Enable privacy compliance dialog method, 
        /// <c>true</c>, enable privacy compliance dialog, <c>false</c> otherwise.
        /// </summary>
        /// <param name="userPrivacyDialog"></param>
        /// <returns></returns>
        public Yodo1AdBuildConfig enableUserPrivacyDialog(bool userPrivacyDialog)
        {
            this._enableUserPrivacyDialog = userPrivacyDialog;
            return this;
        }

        public Yodo1AdBuildConfig userAgreementUrl(string url)
        {
            this._userAgreementUrl = url;
            return this;
        }

        public Yodo1AdBuildConfig privacyPolicyUrl(string url)
        {
            this._privacyPolicyUrl = url;
            return this;
        }

        public string toJson()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("enableAdaptiveBanner", _enableAdaptiveBanner);
            dic.Add("enableUserPrivacyDialog", _enableUserPrivacyDialog);
            if (string.IsNullOrEmpty(_userAgreementUrl))
            {
                dic.Add("userAgreementUrl", "");
            }
            else
            {
                dic.Add("userAgreementUrl", _userAgreementUrl);
            }

            if (string.IsNullOrEmpty(_privacyPolicyUrl))
            {
                dic.Add("privacyPolicyUrl", "");
            }
            else
            {
                dic.Add("privacyPolicyUrl", _privacyPolicyUrl);
            }
            return Yodo1JSON.Serialize(dic);
        }
    }
}



