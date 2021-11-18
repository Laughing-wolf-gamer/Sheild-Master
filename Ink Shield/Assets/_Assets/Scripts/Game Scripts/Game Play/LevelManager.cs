using System;
using UnityEngine;
using GamerWolf.Utils;
using System.Collections.Generic;

namespace InkShield {
    public class LevelManager : MonoBehaviour {

        [SerializeField] private List<LevelDataSO> levelList;
        [SerializeField] private PlayerController player;
        [SerializeField] private MultiTargetCameraController multiTargetCameraController,playerTargetCamera;
        public Action onFirstTouchOnScreen;
        
        private string timerName = "Enemy Check timer";
        private List<EnemyController> enemyList;
        

        #region Singelton.......

        public static LevelManager current;
        private void Awake(){
            if(current == null){
                current = this;
            }else{
                Destroy(current.gameObject);
            }
        }
        #endregion
        

        private void Start(){
            enemyList = new List<EnemyController>();
            
            SpawnLevel();
            

        }
        public void CheckForAllEnemyDead(){
            if(player.GetIsDead()){
                GameHandler.current.isPlayerDead = true;
                PauseEnemyShooting();
                return;
            }
            for (int i = 0; i < enemyList.Count; i++){
                if(!enemyList[i].GetIsDead()){
                    return;
                }
            }
            if(!player.GetIsDead()){
                GameHandler.current.SetGameOver(true);
            }
        }
        private void SpawnLevel(){
            playerTargetCamera.SetTargetToList(player.transform);
            int random = UnityEngine.Random.Range(0,levelList.Count);
            LevelData level =  Instantiate(levelList[random].levelData,transform.position,Quaternion.identity);

            enemyList = level.GetEnemieList();
            SetTargetToList();
            onFirstTouchOnScreen += StartGame;
        }
        private void SetTargetToList(){
            multiTargetCameraController.SetTargetToList(player.transform);
            for (int i = 0; i < enemyList.Count; i++){
                multiTargetCameraController.SetTargetToList(enemyList[i].transform);
            }
        }
        
        private void StartGame(){
            for (int e = 0; e < enemyList.Count; e++){
                enemyList[e].StartEnemy();
            }
            TimerTickSystem.CreateTimer(CheckForAllEnemyDead,0.1f,timerName);
        }
        
        private void PauseEnemyShooting(){
            for (int e = 0; e < enemyList.Count; e++){
                enemyList[e].PlayerDead(player.GetIsDead());
            }

        }
        public void OnPlayerRevive(){
            player.FillInk();
            for (int e = 0; e < enemyList.Count; e++){
                enemyList[e].PlayerDead(player.GetIsDead());
            }
        }
        
        public void EndGame(){
            for (int e = 0; e < enemyList.Count; e++){
                enemyList[e].EndGame();
            }
            TimerTickSystem.StopTimer(timerName);
        }
        public void SubscribeToOnFirstTouch(){
            if(onFirstTouchOnScreen != null){
                onFirstTouchOnScreen();
            }
        }
        
        
    }

}