using UnityEngine;
using Cinemachine;
using System.Collections.Generic;
public enum LevelDefficultiStage {
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
        private List<EnemyController> enemiesList;
        private EnemyController newEnemy;
        private LevelDefficultiStage currentDefficultiy;
        private EnemyController enemyToSpawn;
        private int spawnAmount = 1;
        private List<Vector3> currentSpawnPointsList;
        private int currentEnemyIndex;
        private void Start(){
            LevelManager.current.SetGameViewcamera(gameViewCamera);
        }
        public List<EnemyController> GetEnemieList(){
            return enemiesList;
        }

        private int GetSpawnCount(){
            if(playerDataSO.GetLevelNumber() <= 5){
                int amount = UnityEngine.Random.Range(1,3);
                return amount;
            }
            // if(playerDataSO.GetLevelNumber() > 5){
            //     int amount = UnityEngine.Random.Range(0,3);
            //     return amount;
            // }
            if(playerDataSO.GetLevelNumber() > 15){
                int amount = UnityEngine.Random.Range(1,5);
                return amount;
                // return 3;
            }
            return Random.Range(2,spawnPointList.Count);
        }
        public void SpawnEnemyes(int spawnIndex = -1){
            enemiesList = new List<EnemyController>();
            newEnemy = null;
            currentSpawnPointsList = new List<Vector3>();
            if(playerDataSO.GetLevelNumber() <= 24){
                currentEnemyIndex = 0;
                currentDefficultiy = LevelDefficultiStage.Stage_1;
            }
            if(playerDataSO.GetLevelNumber() >= 25){
                currentEnemyIndex = 1;
                currentDefficultiy = LevelDefficultiStage.Stage_2;
            }
            if(playerDataSO.GetLevelNumber() > 49){
                currentEnemyIndex = 2;
                currentDefficultiy = LevelDefficultiStage.Stage_3;
            }
            if(playerDataSO.GetLevelNumber() >= 55){
                currentDefficultiy = LevelDefficultiStage.Stage_4;
            }
            spawnAmount = GetSpawnCount();
            
            if(spawnAmount > spawnPointList.Count){
                spawnAmount = 2;
            }
            if(spawnAmount <= 0){
                spawnAmount = 1;
            }
            if(spawnIndex == 2){
                spawnAmount = 3;
            }

            Debug.Log("Spawn Count "+ spawnAmount);
            for (int i = 0; i < spawnAmount; i++){
                
                switch(currentDefficultiy){
                    case LevelDefficultiStage.Stage_1:
                        newEnemy = Instantiate(enemyPrefabArray[currentEnemyIndex],spawnPointList[i].position,Quaternion.identity);
                        SetcurrentSpawnPoint(spawnPointList[i]);
                    break;
                    case LevelDefficultiStage.Stage_2:
                        if(spawnIndex >= 0){
                            int currentIndex = (i == 2 ? spawnIndex : i);
                            newEnemy = Instantiate(enemyPrefabArray[currentEnemyIndex],spawnPointList[currentIndex].position,Quaternion.identity);
                            SetcurrentSpawnPoint(spawnPointList[currentIndex]);
                            currentIndex = -1;
                            spawnIndex = -1;
                            Debug.Log("new Enemy is " + newEnemy);
                            continue;
                        }else{
                            newEnemy = Instantiate(enemyPrefabArray[currentEnemyIndex],spawnPointList[i].position,Quaternion.identity);
                            SetcurrentSpawnPoint(spawnPointList[i]);
                            Debug.Log("new Enemy is " + newEnemy);
                        }
                    break;
                    case LevelDefficultiStage.Stage_3:
                        if(spawnIndex >= 0){
                            int currentIndex = (i == 2 ? spawnIndex : i);
                            newEnemy = Instantiate(enemyPrefabArray[currentEnemyIndex],spawnPointList[currentIndex].position,Quaternion.identity);
                            SetcurrentSpawnPoint(spawnPointList[currentIndex]);
                            currentIndex = -1;
                            spawnIndex = -1;
                            Debug.Log("new Enemy is " + newEnemy);
                            continue;
                        }else{
                            Vector3 newSpawnPoint = spawnPointList[i].position;
                            newEnemy = Instantiate(enemyPrefabArray[currentEnemyIndex],newSpawnPoint,Quaternion.identity);
                            SetcurrentSpawnPoint(newSpawnPoint);
                            Debug.Log("new Enemy is " + newEnemy);
                        }
                    break;

                    case LevelDefficultiStage.Stage_4:
                        int randEnemy = UnityEngine.Random.Range(0,enemyPrefabArray.Length);
                        Vector3 spawnPoint = spawnPointList[i].position;
                        newEnemy = Instantiate(enemyPrefabArray[randEnemy],spawnPoint,Quaternion.identity);
                        SetcurrentSpawnPoint(spawnPoint);
                    break;
                }
                if(!enemiesList.Contains(newEnemy)){
                    enemiesList.Add(newEnemy);
                }
                newEnemy.transform.SetParent(transform);
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
        public void SpawnEnemyes(List<Vector3> points,int _prviousSpawnAmount){
            enemiesList = new List<EnemyController>();
            currentSpawnPointsList = new List<Vector3>();
            if(playerDataSO.GetLevelNumber() <= 24){
                
                currentDefficultiy = LevelDefficultiStage.Stage_1;
            }
            if(playerDataSO.GetLevelNumber() > 24){
                
                currentDefficultiy = LevelDefficultiStage.Stage_2;
            }
            if(playerDataSO.GetLevelNumber() > 49){
                
                currentDefficultiy = LevelDefficultiStage.Stage_3;
            }
            if(playerDataSO.GetLevelNumber() >= 80){
                currentDefficultiy = LevelDefficultiStage.Stage_4;
            }
            spawnAmount = _prviousSpawnAmount;

            Debug.Log("Priviouse spawn Count "+ _prviousSpawnAmount);
            for (int i = 0; i < spawnAmount; i++){
                switch(currentDefficultiy){
                    case LevelDefficultiStage.Stage_1:
                        newEnemy = Instantiate(enemyPrefabArray[0],points[i],Quaternion.identity);
                        SetcurrentSpawnPoint(points[i]);
                    break;
                    case LevelDefficultiStage.Stage_2:
                        newEnemy = Instantiate(enemyPrefabArray[1],points[i],Quaternion.identity);
                        SetcurrentSpawnPoint(points[i]);
                    break;
                    case LevelDefficultiStage.Stage_3:
                        newEnemy = Instantiate(enemyPrefabArray[2],points[i],Quaternion.identity);
                        SetcurrentSpawnPoint(points[i]);
                    break;

                    case LevelDefficultiStage.Stage_4:
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
        // public void SpawnTutorialLevels(int levelIndex){
        //     enemiesList = new List<EnemyController>();
        //     newEnemy = null;
        //     currentSpawnPointsList = new List<Vector3>();
        //     if(playerDataSO.GetLevelNumber() <= 24){
        //         currentEnemyIndex = 0;
        //         currentDefficultiy = LevelDefficultiStage.Stage_1;
        //     }
        //     if(playerDataSO.GetLevelNumber() >= 25){
        //         currentEnemyIndex = 1;
        //         currentDefficultiy = LevelDefficultiStage.Stage_2;
        //     }
        //     if(playerDataSO.GetLevelNumber() > 49){
        //         currentEnemyIndex = 2;
        //         currentDefficultiy = LevelDefficultiStage.Stage_3;
        //     }
        //     if(playerDataSO.GetLevelNumber() >= 55){
        //         currentDefficultiy = LevelDefficultiStage.Stage_4;
        //     }
        //     spawnAmount = GetSpawnCount();
            
        //     if(spawnAmount > spawnPointList.Count){
        //         spawnAmount = 2;
        //     }
        //     if(spawnAmount <= 0){
        //         spawnAmount = 1;
        //     }
            

        //     Vector3 newSpawnPoint = spawnPointList[levelIndex].position;
        //     newEnemy = Instantiate(enemyPrefabArray[currentEnemyIndex],spawnPointList[levelIndex].position,Quaternion.identity);
        //     SetcurrentSpawnPoint(spawnPointList[levelIndex]);
        //     // currentIndex = -1;
        //     // spawnIndex = -1;
        //     Debug.Log("new Enemy is " + newEnemy);
        //     Debug.Log("Spawn Count "+ spawnAmount);
            
        // }
        // [ContextMenu("Change FOV")]
        // private void ChangeFOV(){
        //     gameViewCamera.m_Lens.FieldOfView = newFOV;
        // }
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