using UnityEngine;
using Cinemachine;
using System.Collections.Generic;
namespace SheildMaster{ 
    public class LevelData : MonoBehaviour {
        
        [Header("Spawing Variables.")]
        // [SerializeField] private EnemyType[] enemiesTypeToSpawn;
        [SerializeField] private EnemyController[] enemyPrefabArray;
        [SerializeField] private List<Transform> spawnPointList;
        [SerializeField] private CinemachineVirtualCamera gameViewCamera;
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
            enemiesList = new List<EnemyController>();
            int spawnAmount = UnityEngine.Random.Range(1,spawnPointList.Count);
            for (int i = 0; i < spawnAmount; i++){
                int randEnemy = UnityEngine.Random.Range(0,enemyPrefabArray.Length);
                newEnemy = Instantiate(enemyPrefabArray[randEnemy],spawnPointList[i].position,Quaternion.identity);
                newEnemy.transform.SetParent(transform);
                if(!enemiesList.Contains(newEnemy)){
                    enemiesList.Add(newEnemy);
                }
                
            }
            
        }
        
    }

}