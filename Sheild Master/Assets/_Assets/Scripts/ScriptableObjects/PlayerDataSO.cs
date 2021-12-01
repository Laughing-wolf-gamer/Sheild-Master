using System.IO;
using UnityEngine;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace SheildMaster{
    [CreateAssetMenu(fileName = "New Player Data",menuName = "ScriptableObject/Player Data")]
    public class PlayerDataSO : ScriptableObject {
        
        public PlayerSaveData playerSaveData;
        public AbilitySO[] abiliys;
        public Material currentSkinMaterial;
        
        public void AddCoins(int value){
            playerSaveData.maxCoinCount += value;
        }
        public void ReduceCoins(int value){
            playerSaveData.maxCoinCount -= value;
            if(playerSaveData.maxCoinCount <= 0){
                playerSaveData.maxCoinCount = 0;
            }
        }
        public bool GetCurentDailyBonuShown(){
            return playerSaveData.isShownDailyBonus;
        }
        public void SetDailyBonusAlreadyShown(bool _value){
            playerSaveData.isShownDailyBonus = _value;
        }
        public void SetcurrentDay(int day){
            playerSaveData.currentDay = day;
        }
        public int GetcurrentDay(){
            return playerSaveData.currentDay;
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
        public void AddDimond(int amount){
            playerSaveData.dimondAmount += amount;
        }
        public void UseDimond(int amount){
            playerSaveData.dimondAmount -= amount;
            if(playerSaveData.dimondAmount <= 0){
                playerSaveData.dimondAmount = 0;
            }
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
    public class PlayerSaveData {
        public int currentDay;
        public bool isShownDailyBonus;
        public int dimondAmount;
        public int currentLevelNumber;
        public int maxCoinCount;
        public int currentExperience;
    }

}