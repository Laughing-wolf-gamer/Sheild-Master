using System;
using UnityEngine;
using System.Collections.Generic;

namespace SheildMaster {
    public class LevelManager : MonoBehaviour {

        [SerializeField] private PlayerController player;
        [SerializeField] private List<LevelDataSO> levelList;
        [SerializeField] private CinematicCamera cinematicCamera;

        public Action onFirstTouchOnScreen;
        private List<EnemyController> enemyList;
        private GameHandler gameHandler;
        private float surviveTime;
        private int randLevel;

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
                levelList[randLevel].playerDeathCountOnLevel++;
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
                gameHandler.AddCoin(2);
            }
        }
        public void SetLevelEndResult(){
            
            if(player.GetIsDead()){
                GameEventManager.OnGameLost(levelList[randLevel].name,surviveTime,levelList[randLevel].playerDeathCountOnLevel);
            }else{
                GameEventManager.OnGameWon(levelList[randLevel].name,surviveTime);
            }
        }
        
        private void SpawnLevel(){
            randLevel = UnityEngine.Random.Range(0,levelList.Count);
            LevelData currentLevel =  Instantiate(levelList[randLevel].levelData,transform.position,Quaternion.identity);
            
            enemyList = currentLevel.GetEnemieList();
            onFirstTouchOnScreen += StartGame;
        }
        
        
        private void StartGame(){
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
        public void TryEnableAbilityWindow(){
            UIHandler.current.EnableAbilityWindw(true);
            if(enemyList.Count >= 3){
            }else{
                // UIHandler.current.EnableAbilityWindw(false);
            }
        }
        public void KillOneEnemyBeforePlaying(){
            int rand = UnityEngine.Random.Range(0,enemyList.Count);
            enemyList[rand].TakeHit(10);
            SubscribeToOnFirstTouch();
            
        }
        public void ArmourForPlayer(){
            SubscribeToOnFirstTouch();
            player.ActivateForceField();
            
        }
        
        
        
    }

}