using Yodo1.MAS;
using UnityEngine;
using SheildMaster;
using GamerWolf.Utils;
public class PrivacyAndPoicyHandler : MonoBehaviour {
    [SerializeField] private PlayerDataSO playerData;
    [SerializeField] private SettingsSO settingsData;
    [SerializeField] private AdController adController;

    [SerializeField] private GameObject mainMenuUI,privacyPolicyDialogWindow;
    [SerializeField] private TimeManager timeManager;
    private void Start(){
        if(PlayerPrefs.GetInt("AskConsent") > 0){
            privacyPolicyDialogWindow.SetActive(false);
            adController.gameObject.SetActive(true);
            mainMenuUI.SetActive(true);
            gameObject.SetActive(false);
            adController.InitializeADs();
        }else{
            privacyPolicyDialogWindow.SetActive(true);
            adController.gameObject.SetActive(false);
            mainMenuUI.SetActive(false);
            
        }

    }

    public void Agree(){
        Yodo1U3dMas.SetGDPR(true);
        settingsData.SetPlayerConsent(true);
        StartGame();
    }
    private void StartGame(){
        PlayerPrefs.SetInt("AskConsent",1);
        privacyPolicyDialogWindow.SetActive(false);
        adController.gameObject.SetActive(true);
        mainMenuUI.SetActive(true);
        adController.InitializeADs();
    }
    
    public void DisAgree(){
        Yodo1U3dMas.SetGDPR(false);
        settingsData.SetPlayerConsent(false);
        StartGame();
    }
    public void ShowPolicy(){
        if(settingsData.GetPrivacyAndPolicy() != null){
            Application.OpenURL(settingsData.GetPrivacyAndPolicy());
        }
    }
}
