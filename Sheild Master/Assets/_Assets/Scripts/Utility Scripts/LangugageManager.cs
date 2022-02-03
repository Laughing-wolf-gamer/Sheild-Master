using TMPro;
using UnityEngine;
using Lean.Localization;

namespace SheildMaster{
    public class LangugageManager : MonoBehaviour {
        public enum Languages{
                English,
                Brazilian,
                Mexican,
                Argentina,
                Thai,
                Malaysian,
                French,
                Arabic,
                Russian,

            }
        [SerializeField] private SettingsSO settingsSO;
        [SerializeField] private Languages[] languageArray;
        [SerializeField] private LeanLocalization localization;
        [SerializeField] private LanguageChangeButton[] languageChangeButtons;
        [SerializeField] private Languages currentLanguages;


        private void Start(){
            // RefreshCurrentLangugae();
            localization.CurrentLanguage = languageArray[settingsSO.GetCurrentLanguage()].ToString();
        }
        
        public void RefreshCurrentLangugae(){
            for (int i = 0; i < languageChangeButtons.Length; i++){
                if(i == settingsSO.GetCurrentLanguage()){
                    languageChangeButtons[i].ChangeToLanguage();
                    currentLanguages = languageChangeButtons[i].GetButtonLanguage();
                }
            }
        }
        public void ChangeLanguage(Languages languages){
            for (int i = 0; i < languageArray.Length; i++){
                if(languageArray[i] == languages){
                    int lang = (int)languages;
                    currentLanguages = languages;
                    settingsSO.SetCurrentLanguage(lang);
                    languageChangeButtons[i].SetActiveLanguage(true);
                }else {
                    languageChangeButtons[i].SetActiveLanguage(false);
                }
            }
            
            localization.CurrentLanguage = currentLanguages.ToString();
        }
    }
}
