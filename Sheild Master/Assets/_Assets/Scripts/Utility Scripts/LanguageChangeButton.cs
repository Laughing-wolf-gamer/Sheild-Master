using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Lean.Localization;
namespace SheildMaster{

    public class LanguageChangeButton : MonoBehaviour {
        [SerializeField] private LangugageManager settings;
        [SerializeField] private LangugageManager.Languages langugage;
        [SerializeField] private TextMeshProUGUI languageText;
        [SerializeField] private bool isActiveLanguage;
        private void Start(){
            settings.RefreshCurrentLangugae();
        }
        public void ChangeToLanguage(){
            settings.ChangeLanguage(langugage);
            SetActiveLanguage(true);
        }
        public LangugageManager.Languages GetButtonLanguage(){
            return langugage;
        }
        public void SetActiveLanguage(bool value){
            isActiveLanguage = value;
            if(value){
                languageText.color = Color.green;
            }else{
                languageText.color = Color.white;
            }
        }
        
        
    }

}