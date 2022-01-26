using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

namespace SheildMaster{
    [CreateAssetMenu(fileName = "New Player Data",menuName = "ScriptableObject/Player Data")]
    public class PlayerDataSO : ScriptableObject {
        public Action onCurrencyValueChange,onDimondValueChange;
        
        public PlayerSaveData playerSaveData;
        public AbilitySO[] abiliys;
        public ShopItemSO playerSkinItem;
        public Material temporarySkin;
        private void OnValidate(){
            if(playerSaveData.currentLevelNumber <= 0){
                playerSaveData.currentLevelNumber = 1;
            }
        }
        
        public void AddCoins(int value){
            playerSaveData.maxCoinCount += value;
            onCurrencyValueChange?.Invoke();
        }
        public void ReduceCoins(int value){
            playerSaveData.maxCoinCount -= value;
            if(playerSaveData.maxCoinCount <= 0){
                playerSaveData.maxCoinCount = 0;
            }
            onCurrencyValueChange?.Invoke();
        }
        public void SetLostLevelIndex(int value){
            playerSaveData.lostSceneIndex = value;
        }
        public int GetLostLevelIndex(){
            return playerSaveData.lostSceneIndex;
        }
        
        
        public void SetClamedBonus(bool _value){
            playerSaveData.isClamedDailyBonus = _value;
        }
        public bool GetIsClamedBonus(){
            return playerSaveData.isClamedDailyBonus;
        }
        public void IncreaseCurrentDayNumber(){
            playerSaveData.currentDay++;
            if(playerSaveData.currentDay >= 7){
                playerSaveData.currentDay = 0;
            }
        }
        public void SetTemprorarySkin(ShopItemSO itemSO){
            temporarySkin = itemSO.playerSkin;
        }
        public void SetIsClaimed5XBonus(bool value){
            playerSaveData.isClaimed_5_Times = value;
        }
        public int GetcurrentDay(){
            return playerSaveData.currentDay;
        }
        public int GetCashAmount(){
            return playerSaveData.maxCoinCount;
        }

        public int GetLevelNumber(){
            return playerSaveData.currentLevelNumber;
        }
        public void SetHasAdsInGame(bool value){
            playerSaveData.HasAdsInGame = value;
        }
        public bool GetHasAdsInGame(){
            return playerSaveData.HasAdsInGame;
        }
        public bool GetIsClaimed5X(){
            return playerSaveData.isClaimed_5_Times;
        }
        public void OnLevelComplete(){
            playerSaveData.currentLevelNumber++;
            AnayltyicsManager.current.SetPlayerLevelAnaylytics(playerSaveData.currentLevelNumber);
            if(playerSaveData.currentLevelNumber >= 50){
                PlayGamesController.PostAchivements(GPGSIds.achievement_bronze_defender);
            }
            if(playerSaveData.currentLevelNumber >= 100){
                PlayGamesController.PostAchivements(GPGSIds.achievement_silver_defender);
            }
            if(playerSaveData.currentLevelNumber >= 150){
                PlayGamesController.PostAchivements(GPGSIds.achievement_golden_defender);
            }
        }
        public void AddDimond(int amount){
            playerSaveData.dimondAmount += amount;
            onDimondValueChange?.Invoke();
        }
        public void UseDimond(int amount){
            playerSaveData.dimondAmount -= amount;
            if(playerSaveData.dimondAmount <= 0){
                playerSaveData.dimondAmount = 0;
            }
            onDimondValueChange?.Invoke();
        }
        public int GetDimondCount(){
            return playerSaveData.dimondAmount;
        }
        public void SetKillCouts(int amount){
            playerSaveData.totalKillCounts += amount;
            if(playerSaveData.totalKillCounts >= 1){
                PlayGamesController.PostAchivements(GPGSIds.achievement_first_kill);
            }
            if(playerSaveData.totalKillCounts >= 10){
                PlayGamesController.PostAchivements(GPGSIds.achievement_bronze_killer);
            }
            if(playerSaveData.totalKillCounts >= 100){
                PlayGamesController.PostAchivements(GPGSIds.achievement_silver_killer);
            }
            if(playerSaveData.totalKillCounts >= 1000){
                PlayGamesController.PostAchivements(GPGSIds.achievement_golden_killer);
            }
        }
        public int GetTotalKillCount(){
            return playerSaveData.totalKillCounts;
        }

        // public void SetLostBool(bool value){
        //     this.playerSaveData.lostData.lostOnThisLevel = value;
            
        // }
        public void SetSpawnAmount(int amount){
            this.playerSaveData.lostData.spawnAmount = amount;
        }
        public void SetLostEnemySpawnPointsData(List<Vector3> spawnPointList) {
            this.playerSaveData.lostData.spawnPointlist = spawnPointList;
        }
        public int GetSpawnAmount(){
            return playerSaveData.lostData.spawnAmount;
        }
        // public bool GetIsLostOnLevel(){
        //     return playerSaveData.lostData.lostOnThisLevel;
        // }
        public List<Vector3> GetSpawnPointList(){
            return playerSaveData.lostData.spawnPointlist;
        }
        public void RemakeNewSpawnPoint(){
            playerSaveData.lostData.spawnPointlist = new List<Vector3>();
            SetSpawnAmount(0);
            // SetLostBool(false);
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
        
        public bool HasAdsInGame;
        public bool isClamedDailyBonus;
        public bool isClaimed_5_Times;
        public int dimondAmount;
        public int currentLevelNumber;
        public int maxCoinCount;
        public int totalKillCounts = 0;
        public int currentDay;
        // public int lostLevelIndex;
        public int lostSceneIndex;
        public OnLostPlayerData lostData;
    }
    [System.Serializable]
    public class OnLostPlayerData{
        public int spawnAmount;
        public List<Vector3> spawnPointlist;
    }

}