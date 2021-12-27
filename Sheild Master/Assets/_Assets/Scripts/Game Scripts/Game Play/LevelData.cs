using UnityEngine;
using Cinemachine;
using System.Collections.Generic;
public enum LevelStag {
    Stage_1,Stage_2,Stage_3,Stage_4
}
namespace SheildMaster{ 
    public class LevelData : MonoBehaviour {
        [SerializeField] private float gizmosBoxSize = 2f;
        [SerializeField] private bool spawnOnAllPoint;
        [Header("Spawing Variables.")]
        [SerializeField] private PlayerDataSO playerDataSO;
        [SerializeField] private EnemyController[] enemyPrefabArray;
        [SerializeField] private List<Transform> spawnPointList;
        [SerializeField] private CinemachineVirtualCamera gameViewCamera;
        [SerializeField] private float newFOV = 70;
        private List<EnemyController> enemiesList;
        private EnemyController newEnemy;
        private LevelStag currentStage;
        private EnemyController enemyToSpawn;
        private int spawnAmount = 0;
        private List<Vector3> currentSpawnPointsList;
        private void Awake(){
            
            // SpawnEnemyes();
        }
        private void Start(){
            LevelManager.current.SetGameViewcamera(gameViewCamera);
        }
        public List<EnemyController> GetEnemieList(){
            return enemiesList;
        }

        private int GetSpawnCount(){
            
            
            if(playerDataSO.GetLevelNumber() <= 5){
                
                return 1;
                // Debug.Log("SpawnAmount is " + spawnAmount);
            }
            else if(playerDataSO.GetLevelNumber() > 5 && playerDataSO.GetLevelNumber() <= 15){

                return 2;
            }
            else if(playerDataSO.GetLevelNumber() > 15 && playerDataSO.GetLevelNumber() <= 26){
                
                // Debug.Log("SpawnAmount is " + spawnAmount);
                return 3;

            }else{
                return UnityEngine.Random.Range(1,spawnPointList.Count);
            }
            // if(returnAmount >= spawnPointList.Count){
            //     returnAmount = spawnPointList.Count;
            // }
            // Debug.Log("SpawnAmount is " + spawnAmount);
            
            
            
        }
        public void SpawnEnemyes(){
            enemiesList = new List<EnemyController>();
            newEnemy = null;
            currentSpawnPointsList = new List<Vector3>();
            // if(spawnAmount >= spawnPointList.Count){
            //     spawnAmount = UnityEngine.Random.Range(1,spawnPointList.Count);
            // }
            if(playerDataSO.GetLevelNumber() <= 25){
                
                currentStage = LevelStag.Stage_1;
            }
            if(playerDataSO.GetLevelNumber() > 25){
                
                currentStage = LevelStag.Stage_2;
            }
            if(playerDataSO.GetLevelNumber() >= 60){
                
                currentStage = LevelStag.Stage_3;
            }
            if(playerDataSO.GetLevelNumber() >= 100){
                currentStage = LevelStag.Stage_4;
            }
            // if(!spawnOnAllPoint){
            //     spawnAmount = GetSpawnCount(spawnPointList.Count);
            // }else{
            //     spawnAmount = UnityEngine.Random.Range(1,spawnPointList.Count);
            // }
            if(!spawnOnAllPoint){
                if(spawnAmount > spawnPointList.Count){
                    spawnAmount = spawnPointList.Count;
                }else{
                    spawnAmount = GetSpawnCount();
                }
            }
            Debug.Log("Spawn Count "+ spawnAmount);
            for (int i = 0; i < spawnAmount; i++){
                
                switch(currentStage){
                    case LevelStag.Stage_1:
                        newEnemy = Instantiate(enemyPrefabArray[0],spawnPointList[i].position,Quaternion.identity);
                        SetcurrentSpawnPoint(spawnPointList[i]);
                    break;
                    case LevelStag.Stage_2:
                        newEnemy = Instantiate(enemyPrefabArray[1],spawnPointList[i].position,Quaternion.identity);
                        SetcurrentSpawnPoint(spawnPointList[i]);
                    break;
                    case LevelStag.Stage_3:
                        newEnemy = Instantiate(enemyPrefabArray[2],spawnPointList[i].position,Quaternion.identity);
                        SetcurrentSpawnPoint(spawnPointList[i]);
                    break;

                    case LevelStag.Stage_4:
                        int randEnemy = UnityEngine.Random.Range(0,enemyPrefabArray.Length);
                        newEnemy = Instantiate(enemyPrefabArray[randEnemy],spawnPointList[i].position,Quaternion.identity);
                        SetcurrentSpawnPoint(spawnPointList[i]);
                    break;
                }
                newEnemy.transform.SetParent(transform);
                if(!enemiesList.Contains(newEnemy)){
                    enemiesList.Add(newEnemy);
                }
            }
            
        }
        private void SetcurrentSpawnPoint(Transform points){
            if(!currentSpawnPointsList.Contains(points.position)){
                currentSpawnPointsList.Add(points.position);
            }
        }
        private void SetcurrentSpawnPoint(Vector3 points){
            if(!currentSpawnPointsList.Contains(points)){
                currentSpawnPointsList.Add(points);
            }
        }
        public void SpawnEnemyes(List<Vector3> points,int _spawnAmount){
            enemiesList = new List<EnemyController>();
            currentSpawnPointsList = new List<Vector3>();
            int currentSpawnAmount = _spawnAmount;
            // if(spawnAmount >= spawnPointList.Count){
            //     spawnAmount = UnityEngine.Random.Range(1,spawnPointList.Count);
            // }
            if(playerDataSO.GetLevelNumber() <= 25){
                currentStage = LevelStag.Stage_1;
            }
            if(playerDataSO.GetLevelNumber() > 25){
                currentStage = LevelStag.Stage_2;
            }
            if(playerDataSO.GetLevelNumber() >= 60){
                currentStage = LevelStag.Stage_3;
            }
            if(playerDataSO.GetLevelNumber() >= 100){
                currentStage = LevelStag.Stage_4;
            }
            // if(!spawnOnAllPoint){
            //     spawnAmount = GetSpawnCount(spawnPointList.Count);
            // }else{
            //     spawnAmount = UnityEngine.Random.Range(1,spawnPointList.Count);
            // }
            spawnAmount = _spawnAmount;
            if(!spawnOnAllPoint){
                // currentSpawnAmount = _spawnAmount;
                // if(_spawnAmount > spawnPointList.Count){
                //     currentSpawnAmount = spawnPointList.Count;
                // }else{
                // }
            }
            Debug.Log("Priviouse spawn Count "+ currentSpawnAmount);
            for (int i = 0; i < currentSpawnAmount; i++){

                switch(currentStage){
                    case LevelStag.Stage_1:
                        newEnemy = Instantiate(enemyPrefabArray[0],points[i],Quaternion.identity);
                        SetcurrentSpawnPoint(points[i]);
                    break;
                    case LevelStag.Stage_2:
                        newEnemy = Instantiate(enemyPrefabArray[1],points[i],Quaternion.identity);
                        SetcurrentSpawnPoint(points[i]);
                    break;
                    case LevelStag.Stage_3:
                        newEnemy = Instantiate(enemyPrefabArray[2],points[i],Quaternion.identity);
                        SetcurrentSpawnPoint(points[i]);
                    break;

                    case LevelStag.Stage_4:
                        int randEnemy = UnityEngine.Random.Range(0,enemyPrefabArray.Length);
                        newEnemy = Instantiate(enemyPrefabArray[randEnemy],points[i],Quaternion.identity);
                        SetcurrentSpawnPoint(points[i]);
                    break;
                }
                newEnemy.transform.SetParent(transform);
                if(!enemiesList.Contains(newEnemy)){
                    enemiesList.Add(newEnemy);
                }
            }
        }
        [ContextMenu("Change FOV")]
        private void ChangeFOV(){
            gameViewCamera.m_Lens.FieldOfView = newFOV;
        }
        public List<Vector3> GetSpawnPoint(){
            return currentSpawnPointsList;
        }
        public int previousSpawnCount(){
            return spawnAmount;
        }
        
        
        private void OnDrawGizmos(){
            if(spawnPointList.Count > 0){
                for (int i = 0; i < spawnPointList.Count; i++){
                    Gizmos.color = Color.red;
                    Gizmos.DrawCube(spawnPointList[i].position,Vector3.one * gizmosBoxSize);
                }
            }
        }
        
    }

}