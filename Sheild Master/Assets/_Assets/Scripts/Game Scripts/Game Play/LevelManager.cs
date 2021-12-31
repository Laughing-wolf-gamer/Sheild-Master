using System;
using UnityEngine;
using System.Collections.Generic;

namespace SheildMaster {
    public class LevelManager : MonoBehaviour {
        [SerializeField] private PlayerDataSO playerDataSO;
        [SerializeField] private PlayerController player;
        [SerializeField] private List<LevelDataSO> levelDataList;
        [SerializeField] private CinematicCamera cinematicCamera;
        [SerializeField] private Camera UiCamera;
        [SerializeField] private bool isTouchedtheScreen;
        [SerializeField] private AbilitySO sheildAbility,KillAbility;
        private List<EnemyController> enemyList;
        private GameHandler gameHandler;
        private float surviveTime;
        private int randLevel;
        private int currenCoinCount;
        private LevelDataSO currentUsedLevelDataSO;
        private LevelData currentLevel;
        private UIHandler uIHandler;


        #region Singelton.......

        public static LevelManager current;
        private void Awake(){
            if(current == null){
                current = this;
            }else{
                Destroy(current.gameObject);
            }
            gameHandler = GetComponent<GameHandler>();
        }
        #endregion
        

        private void Start(){
            uIHandler = UIHandler.current;
            
            enemyList = new List<EnemyController>();
            if(playerDataSO.GetLevelNumber() == 1){
                SpawnForLevel1();
            }else {
                SpawnLevel();
            }

        }
        public void CheckForAllEnemyDead(){
            surviveTime ++;
            if(player.GetIsDead()){
                levelDataList[randLevel].playerDeathCountOnLevel++;
                gameHandler.SetGameOver(false);
                playerDataSO.SetLostLevelIndex(levelDataList.IndexOf(currentUsedLevelDataSO));
                SetLostLevel();
                return;
            }
            for (int i = 0; i < enemyList.Count; i++){
                if(!enemyList[i].GetIsDead()){
                    return;
                }
            }
            if(!player.GetIsDead()){
                gameHandler.SetGameOver(true);
                currenCoinCount = enemyList.Count;
                playerDataSO.SetLostLevelIndex(-1);
                UIHandler.current.SetCurrentLevelEarnedCoins(currenCoinCount);    
                SetWinLevel();
                if(playerDataSO.GetLevelNumber() >= 25){
                    sheildAbility.IncreaseAbility(2);
                }
                if(playerDataSO.GetLevelNumber() >= 50){
                    KillAbility.IncreaseAbility(2);
                }
            }
        }
        public void CollectCoin(){
            gameHandler.AddCoin(currenCoinCount);
            UIHandler.current.UpdateCoinAmountUI();
        }
        public void AddTwiceMoney(){
            currenCoinCount = currenCoinCount * 2;
            UIHandler.current.SetCurrentLevelEarnedCoins(currenCoinCount);
            UIHandler.current.UpdateCoinAmountUI();
        }
        
        public void SetLevelEndResult(){
            if(player.GetIsDead()){
                AnayltyicsManager.current.OnGameLost_UnityAnayltics(levelDataList[randLevel].name,surviveTime,levelDataList[randLevel].playerDeathCountOnLevel);
            }else{
                AnayltyicsManager.current.OnGameWon_UnityAnayltics(levelDataList[randLevel].name,surviveTime);
            }
        }
        private void SpawnForLevel1(){
            currentUsedLevelDataSO = levelDataList[0];
            if(!currentUsedLevelDataSO.GetIsLostOnLevel()){
                currentLevel = Instantiate(currentUsedLevelDataSO.levelData,transform.position,Quaternion.identity);
                currentLevel.SpawnEnemyes();
            }else{
                currentLevel =  Instantiate(currentUsedLevelDataSO.levelData,transform.position,Quaternion.identity);
                currentLevel.SpawnEnemyes(currentUsedLevelDataSO.GetSpawnPointList(),currentUsedLevelDataSO.GetSpawnAmount());
            }
            
            enemyList = currentLevel.GetEnemieList();
            for (int i = 0; i < enemyList.Count; i++){
                enemyList[i].SetViewCameraForHealthBar(UiCamera);
            }
            currenCoinCount = enemyList.Count;
            currentUsedLevelDataSO.RemakeNewSpawnPoint();
        }
        private void SpawnLevel(){
            randLevel = UnityEngine.Random.Range(0,levelDataList.Count);
            if(playerDataSO.GetLostLevelIndex() != -1){
                if(levelDataList[playerDataSO.GetLostLevelIndex()].GetIsLostOnLevel()){
                    SpawnSameLevel();
                }else{
                    SpawnRandomLevel();
                }
            }else{
                SpawnRandomLevel();  
            }            
            enemyList = currentLevel.GetEnemieList();
            for (int i = 0; i < enemyList.Count; i++){
                enemyList[i].SetViewCameraForHealthBar(UiCamera);
            }
            currenCoinCount = enemyList.Count;
            currentUsedLevelDataSO.RemakeNewSpawnPoint();
        }
        private void SpawnSameLevel(){
            currentUsedLevelDataSO = levelDataList[playerDataSO.GetLostLevelIndex()];
            currentLevel =  Instantiate(currentUsedLevelDataSO.levelData,transform.position,Quaternion.identity);
            currentLevel.SpawnEnemyes(currentUsedLevelDataSO.GetSpawnPointList(),currentUsedLevelDataSO.GetSpawnAmount());
        }
        private void SpawnRandomLevel(){
            currentUsedLevelDataSO = levelDataList[randLevel];
            currentLevel = Instantiate(currentUsedLevelDataSO.levelData,transform.position,Quaternion.identity);
            currentLevel.SpawnEnemyes();
        }
        
        public void StartGame(){
            isTouchedtheScreen = true;
            for (int e = 0; e < enemyList.Count; e++){
                enemyList[e].StartEnemy();
            }
        }
        
        public void SetGameViewcamera(Cinemachine.CinemachineVirtualCamera camer){
            cinematicCamera.SetGameCamera(camer);
        }
        
        
        public void EndGame(){
            for (int e = 0; e < enemyList.Count; e++){
                enemyList[e].EndGame();
            }
        }
        
        public void CheckifCanOpenAbiltiyPanal(){
            if(playerDataSO.GetLevelNumber() < 25){
                uIHandler.HideSheildAbilityIcon(true);
            }else{
                uIHandler.HideSheildAbilityIcon(false);
            }
            if(playerDataSO.GetLevelNumber() < 50){
                uIHandler.HideKillAbilityIcon(true);
            }else{
                uIHandler.HideKillAbilityIcon(false);
            }
            
        }
        
        public void KillOneEnemyBeforePlaying(){
            
            int rand = UnityEngine.Random.Range(0,enemyList.Count);
            if(enemyList[rand].GetIsDead()){
                rand = UnityEngine.Random.Range(0,enemyList.Count);
            }
            enemyList[rand].TakeHit(20);
            StartGame();
        }
        public void ArmourForPlayer(){
            StartGame();
            player.ActivateForceField();
        }
        private void SetLostLevel(){
            if(gameHandler.GetIslost()){
                currentUsedLevelDataSO.SetSpawnAmount(currentLevel.previousSpawnCount());
                currentUsedLevelDataSO.SetLostBool(true) ;
                currentUsedLevelDataSO.SetLostData(currentLevel.GetSpawnPoint());
            }
        }
        private void SetWinLevel(){
            if(gameHandler.GetIsWon()){
                currentUsedLevelDataSO.SetLostBool(false);
                currentUsedLevelDataSO.RemakeNewSpawnPoint();
                currentUsedLevelDataSO.SetSpawnAmount(0);
            }
        }
        
        
    }

}