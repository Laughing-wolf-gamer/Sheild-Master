using System.IO;
using UnityEngine;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
namespace InkShield {
    [CreateAssetMenu(fileName = "New Setting",menuName = "ScriptableObject/UI/Settings Data")]
    public class SettingsSO : ScriptableObject {
        public string savePlace = "Settings Data";
        public SettingsData settingsData;

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
    public class SettingsData{
        public int currentLanguage;
        public bool isMusicOn;
        public bool isSoundOn;
        public string privacyAndPolicy;
    }
    

}