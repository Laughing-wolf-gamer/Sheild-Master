using UnityEngine;
using Cinemachine;
using System.Collections.Generic;
namespace SheildMaster{ 
    public class LevelData : MonoBehaviour {
        [SerializeField] private float gizmosSize = 2f;
        [SerializeField] private bool spawnOnAllPoint;
        [Header("Spawing Variables.")]
        [SerializeField] private PlayerDataSO playerDataSO;
        [SerializeField] private EnemyController[] enemyPrefabArray;
        [SerializeField] private List<Transform> spawnPointList;
        [SerializeField] private CinemachineVirtualCamera gameViewCamera;
        [SerializeField] private float newFOV = 70;
        private List<EnemyController> enemiesList;
        private EnemyController newEnemy;
        private void Awake(){
            SpawnEnemyes();
        }
        private void Start(){
            LevelManager.current.SetGameViewcamera(gameViewCamera);
        }
        public List<EnemyController> GetEnemieList(){
            return enemiesList;
        }

        
        private void SpawnEnemyes(){
            int randEnemy = 0;
            enemiesList = new List<EnemyController>();
            int spawnAmount = 0;
            if(!spawnOnAllPoint){
                spawnAmount = UnityEngine.Random.Range(1,spawnPointList.Count);
            }else{
                spawnAmount = spawnPointList.Count;
            }
            for (int i = 0; i < spawnAmount; i++){
                if(playerDataSO.GetLevelNumber() <= 10){
                    randEnemy = 0;
                }else if(playerDataSO.GetLevelNumber() > 10 && playerDataSO.GetLevelNumber() <= 20){
                    randEnemy = UnityEngine.Random.Range(1,enemyPrefabArray.Length);
                }else if(playerDataSO.GetLevelNumber() > 20 && playerDataSO.GetLevelNumber() <= 25){
                    randEnemy = 2;
                }else{
                    randEnemy = UnityEngine.Random.Range(0,enemyPrefabArray.Length);
                }
                newEnemy = Instantiate(enemyPrefabArray[randEnemy],spawnPointList[i].position,Quaternion.identity);
                

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
        
        private void OnDrawGizmos(){
            if(spawnPointList.Count > 0){
                for (int i = 0; i < spawnPointList.Count; i++){
                    Gizmos.color = Color.red;
                    Gizmos.DrawCube(spawnPointList[i].position,Vector3.one * gizmosSize);
                }
            }
        }
        
    }

}