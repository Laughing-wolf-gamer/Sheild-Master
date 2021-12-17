using System;
using UnityEngine;
using System.Collections.Generic;

namespace SheildMaster {
    public class LevelManager : MonoBehaviour {

        [SerializeField] private PlayerController player;
        [SerializeField] private List<LevelDataSO> levelDataList;
        [SerializeField] private CinematicCamera cinematicCamera;
        [SerializeField] private Camera UiCamera;
        [SerializeField] private bool isTouchedtheScreen;

        public Action onFirstTouchOnScreen;
        private List<EnemyController> enemyList;
        private GameHandler gameHandler;
        private float surviveTime;
        private int randLevel;
        private int currenCoinCount;
        private LevelDataSO currentUsedLevelDataSO;
        private LevelData currentLevel;

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
            enemyList = new List<EnemyController>();
            SpawnLevel();
        }
        public void CheckForAllEnemyDead(){
            surviveTime ++;
            if(player.GetIsDead()){
                levelDataList[randLevel].playerDeathCountOnLevel++;
                gameHandler.SetGameOver(false);
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
                UIHandler.current.SetCurrentLevelEarnedCoins(currenCoinCount);    
            }
        }
        public void CollectCoin(){
            // if(player.GetTouchCount() == 1){
            //     gameHandler.AddCoin(enemyList.Count);    
            // }
            UIHandler.current.SetCurrentLevelEarnedCoins(0);
            gameHandler.AddCoin(currenCoinCount);
            UIHandler.current.UpdateCoinAmountUI();
        }
        public void AddTwiceMoney(){
            currenCoinCount = currenCoinCount * 2;
            gameHandler.AddCoin(currenCoinCount);
            UIHandler.current.UpdateCoinAmountUI();
        }
        
        public void SetLevelEndResult(){
            
            if(player.GetIsDead()){
                AnayltyicsManager.current.OnGameLost_UnityAnayltics(levelDataList[randLevel].name,surviveTime,levelDataList[randLevel].playerDeathCountOnLevel);
            }else{
                AnayltyicsManager.current.OnGameWon_UnityAnayltics(levelDataList[randLevel].name,surviveTime);
            }
        }
        
        private void SpawnLevel(){
            randLevel = UnityEngine.Random.Range(0,levelDataList.Count);
            currentUsedLevelDataSO = levelDataList[randLevel];
            for (int i = 0; i < levelDataList.Count; i++){
                if(levelDataList[i].GetIsLostOnLevel()){
                    currentUsedLevelDataSO = levelDataList[i];
                    break;
                }
            }
            
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
        public void SubscribeToOnFirstTouch(){
            if(onFirstTouchOnScreen != null){
                
                onFirstTouchOnScreen();
            }

        }
        
        public void KillOneEnemyBeforePlaying(){
            int rand = UnityEngine.Random.Range(0,enemyList.Count);
            if(enemyList[rand].GetIsDead()){
                rand = UnityEngine.Random.Range(0,enemyList.Count);
            }
            enemyList[rand].TakeHit(50);
            SubscribeToOnFirstTouch();
            
        }
        public void ArmourForPlayer(){
            SubscribeToOnFirstTouch();
            player.ActivateForceField();
            
        }
        public void SetLostLevel(){
            
            if(gameHandler.GetIslost()){
                currentUsedLevelDataSO.SetSpawnAmount(currentLevel.previousSpawnCount());
                currentUsedLevelDataSO.SetLostBool(true) ;
                currentUsedLevelDataSO.SetLostData(currentLevel.GetSpawnPoint());
            }
        }
        public void SetWinLevel(){
            if(gameHandler.GetIsWon()){
                currentUsedLevelDataSO.SetLostBool(false);
                currentUsedLevelDataSO.RemakeNewSpawnPoint();
                currentUsedLevelDataSO.SetSpawnAmount(0);
            }
        }
        
        
    }

}