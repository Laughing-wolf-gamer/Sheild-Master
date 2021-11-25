using System.IO;
using UnityEngine;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace InkShield{
    [CreateAssetMenu(fileName = "New Player Data",menuName = "ScriptableObject/Player Data")]
    public class PlayerDataSO : ScriptableObject {
        
        public PlayerSaveData playerSaveData;
        public AbilitySO[] abiliys;
        public void AddCoins(int value){
            playerSaveData.maxCoinCount += value;
        }
        public void ReduceCoins(int value){
            playerSaveData.maxCoinCount -= value;
            if(playerSaveData.maxCoinCount <= 0f){
                playerSaveData.maxCoinCount = 0;
            }
        }
        public int GetCoinValue(){
            return playerSaveData.maxCoinCount;
        }
        public int GetExperience(){
            return playerSaveData.currentExperience;
        }
        public int GetLevelNumber(){
            return playerSaveData.currentLevelNumber;
        }
        public void OnLevelComplete(){
            playerSaveData.currentLevelNumber++;
        }
        [ContextMenu("Save")]
        public void Save(){
            for (int i = 0; i < abiliys.Length; i++){
                abiliys[i].Save();
            }
            string data = JsonUtility.ToJson(playerSaveData,true);
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Create(string.Concat(Application.persistentDataPath,"/","Player Data"));
            formatter.Serialize(file,data);
            file.Close();
        }

        [ContextMenu("Load")]
        public void Load(){
            for (int i = 0; i < abiliys.Length; i++){
                abiliys[i].Load();
            }
            if(File.Exists((string.Concat(Application.persistentDataPath,"/","Player Data")))){
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream Stream = File.Open(string.Concat(Application.persistentDataPath,"/","Player Data"),FileMode.Open);
                JsonUtility.FromJsonOverwrite(formatter.Deserialize(Stream).ToString(),playerSaveData);
                Stream.Close();
            }
        }
        
    }
    [System.Serializable]
    public class PlayerSaveData{
        public int currentLevelNumber;
        public int maxCoinCount;
        public int currentExperience;
    }

}