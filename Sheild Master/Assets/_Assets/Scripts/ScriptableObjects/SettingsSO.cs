using System.IO;
using UnityEngine;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace SheildMaster {
    [CreateAssetMenu(fileName = "New Setting",menuName = "ScriptableObject/UI/Settings Data")]
    public class SettingsSO : ScriptableObject {
        public string savePlace = "Settings Data";
        [SerializeField] private SettingsData settingsData;

        public bool GetIsMusicOn(){
            return settingsData.isMusicOn;
        }
        public bool GetIsSoundOn(){
            return settingsData.isSoundOn;
        }
        public bool GetNotificationOn(){
            return settingsData.isNotificaitonOn;
        }
        
        public void SetNotificaiton(){
            settingsData.isNotificaitonOn = !settingsData.isNotificaitonOn;
        }
        public void SetMusic(){
            settingsData.isMusicOn = !settingsData.isMusicOn;
        }
        public void SetSound(){
            settingsData.isSoundOn = !settingsData.isSoundOn;
        }
        public int GetCurrentLanguage(){
            return settingsData.currentLanguage;
        }
        public void SetCurrentLanguage(int value){
            settingsData.currentLanguage = value;
        }
        public void SetPlayerConsent(bool value){
            settingsData.hasPlayerConsent = value;
        }
        public bool GetPlayerConsent(){
            return settingsData.hasPlayerConsent;
        }
        public string GetPrivacyAndPolicy(){
            return settingsData.privacyAndPolicy;
        }
        [ContextMenu("Save")]
        public void Save(){
            
            string data = JsonUtility.ToJson(settingsData,true);
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Create(string.Concat(Application.persistentDataPath,"/",savePlace,".dat"));
            formatter.Serialize(file,data);
            file.Close();
        }

        [ContextMenu("Load")]
        public void Load(){
            
            if(File.Exists((string.Concat(Application.persistentDataPath,"/",savePlace,".dat")))){
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream Stream = File.Open(string.Concat(Application.persistentDataPath,"/",savePlace,".dat"),FileMode.Open);
                JsonUtility.FromJsonOverwrite(formatter.Deserialize(Stream).ToString(),settingsData);
                Stream.Close();
            }
        }
    }
    [System.Serializable]
    public class SettingsData {
        public bool hasPlayerConsent;
        public int currentLanguage;
        public bool isNotificaitonOn;
        public bool isMusicOn;
        public bool isSoundOn;
        public string privacyAndPolicy;
    }
    

}