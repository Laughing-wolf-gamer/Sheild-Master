using System;
using UnityEngine;
using System.Collections.Generic;

namespace SheildMaster {
    public class LevelManager : MonoBehaviour {

        [SerializeField] private PlayerController player;
        [SerializeField] private List<LevelDataSO> levelDataList;
        [SerializeField] private CinematicCamera cinematicCamera;
        [SerializeField] private Camera UiCamera;

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
            }
        }
        public void CollectCoin(){
            if(player.GetTouchCount() == 1){
                gameHandler.AddCoin(enemyList.Count + 2);    
            }
            gameHandler.AddCoin(enemyList.Count);
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
            LevelData currentLevel =  Instantiate(levelDataList[randLevel].levelData,transform.position,Quaternion.identity);
            
            enemyList = currentLevel.GetEnemieList();
            for (int i = 0; i < enemyList.Count; i++){
                enemyList[i].SetViewCameraForHealthBar(UiCamera);
            }
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
            if(enemyList.Count >= 3){
                UIHandler.current.EnableAbilityWindow(true);
            }else{
                UIHandler.current.EnableAbilityWindow(false);
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
        
        
        
    }

}