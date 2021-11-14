using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GamerWolf.Utils;

namespace InkShield {
    public class LevelManager : MonoBehaviour {

        [SerializeField] private List<LevelDataSO> levelList;
        [SerializeField] private List<EnemyController> enemiesList;
        [SerializeField] private PlayerController player;
        public Action onFirstTouchOnScreen;
        private string timerName = "Enemy Check timer";


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
            enemiesList = new List<EnemyController>();
            SpawnLevel();
            onFirstTouchOnScreen += StartGame;

        }
        public void CheckForAllEnemyDead(){
            if(player.GetIsDead()){
                GameHandler.current.SetGameOver(false);
                return;
            }
            for (int i = 0; i < enemiesList.Count; i++){
                if(!enemiesList[i].GetIsDead()){
                    return;
                }
            }
            if(!player.GetIsDead()){
                GameHandler.current.SetGameOver(true);
            }
        }
        private void SpawnLevel(){
            int random = UnityEngine.Random.Range(0,levelList.Count);
            LevelData level =  Instantiate(levelList[random].levelData,transform.position,Quaternion.identity);
            enemiesList = level.GetEnemieList();
            for (int i = 0; i < enemiesList.Count; i++){
                MultiTargetCameraController.current.SetTargetToList(player.transform);
                MultiTargetCameraController.current.SetTargetToList(enemiesList[i].transform);
            }
        }
        private void StartGame(){
            for (int e = 0; e < enemiesList.Count; e++){
                enemiesList[e].StartGame();
            }
            TimerTickSystem.CreateTimer(CheckForAllEnemyDead,0.2f,timerName);
        }
        public void EndGame(){
            for (int e = 0; e < enemiesList.Count; e++){
                enemiesList[e].EndGame();
            }
            TimerTickSystem.StopTimer(timerName);

        }
        public void SubscribeToOnFirstTouch(){
            onFirstTouchOnScreen?.Invoke();
        }
        
        
    }

}