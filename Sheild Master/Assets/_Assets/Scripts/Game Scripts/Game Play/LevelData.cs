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
        // [SerializeField] private float newFOV = 70;
        private List<EnemyController> enemiesList;
        private EnemyController newEnemy;
        private LevelDefficultiStage currentDefficultiy;
        private EnemyController enemyToSpawn;
        private int spawnAmount = 0;
        private List<Vector3> currentSpawnPointsList;
        
        private void Start(){
            LevelManager.current.SetGameViewcamera(gameViewCamera);
        }
        public List<EnemyController> GetEnemieList(){
            return enemiesList;
        }

        private int GetSpawnCount(){
            if(playerDataSO.GetLevelNumber() <= 5){
                return 1;
            }
            if(playerDataSO.GetLevelNumber() > 5 && playerDataSO.GetLevelNumber() <= 15){

                return 2;
            }
            if(playerDataSO.GetLevelNumber() > 15 && playerDataSO.GetLevelNumber() <= 25){
                return 3;

            }
            return Random.Range(2,spawnPointList.Count);
        }
        public void SpawnEnemyes(int spawnIndex = -1){
            enemiesList = new List<EnemyController>();
            newEnemy = null;
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
            spawnAmount = GetSpawnCount();
            
            if(spawnAmount > spawnPointList.Count){
                spawnAmount = spawnPointList.Count;
            }
            if(spawnIndex == 2){
                spawnAmount = 3;
            }
            if(spawnAmount <= 0){
                spawnAmount = 1;
            }

            Debug.Log("Spawn Count "+ spawnAmount);
            for (int i = 0; i < spawnAmount; i++){
                
                switch(currentDefficultiy){
                    case LevelDefficultiStage.Stage_1:
                        newEnemy = Instantiate(enemyPrefabArray[0],spawnPointList[i].position,Quaternion.identity);
                        SetcurrentSpawnPoint(spawnPointList[i]);
                    break;
                    case LevelDefficultiStage.Stage_2:
                        if(spawnIndex > 0){
                            int currentIndex = (i == 2 ? spawnIndex : i);
                            newEnemy = Instantiate(enemyPrefabArray[1],spawnPointList[currentIndex].position,Quaternion.identity);
                            SetcurrentSpawnPoint(spawnPointList[currentIndex]);
                            // continue;
                        }else{
                            newEnemy = Instantiate(enemyPrefabArray[1],spawnPointList[1].position,Quaternion.identity);
                            SetcurrentSpawnPoint(spawnPointList[i]);
                        }
                    break;
                    case LevelDefficultiStage.Stage_3:
                        if(spawnIndex > 0){
                            int currentIndex = (i == 2 ? spawnIndex : i);
                            newEnemy = Instantiate(enemyPrefabArray[2],spawnPointList[currentIndex].position,Quaternion.identity);
                            SetcurrentSpawnPoint(spawnPointList[currentIndex]);
                            // continue;
                        }else{
                            newEnemy = Instantiate(enemyPrefabArray[2],spawnPointList[i].position,Quaternion.identity);
                            SetcurrentSpawnPoint(spawnPointList[i]);
                        }
                    break;

                    case LevelDefficultiStage.Stage_4:
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
        public void SpawnEnemyes(List<Vector3> points,int _prviousSpawnAmount){
            enemiesList = new List<EnemyController>();
            currentSpawnPointsList = new List<Vector3>();
            
            spawnAmount = _prviousSpawnAmount;
            if(playerDataSO.GetLevelNumber() <= 25){
                currentDefficultiy = LevelDefficultiStage.Stage_1;
            }
            if(playerDataSO.GetLevelNumber() > 25){
                
                currentDefficultiy = LevelDefficultiStage.Stage_2;
            }
            if(playerDataSO.GetLevelNumber() > 60){
                
                currentDefficultiy = LevelDefficultiStage.Stage_3;
            }
            if(playerDataSO.GetLevelNumber() >= 80){
                currentDefficultiy = LevelDefficultiStage.Stage_4;
            }
            Debug.Log("Priviouse spawn Count "+ spawnAmount);
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
        private void SpawnTutoralLevel(){

        }
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