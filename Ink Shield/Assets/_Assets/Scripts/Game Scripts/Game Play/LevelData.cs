using UnityEngine;
using System.Collections.Generic;
namespace InkShield{ 
    public class LevelData : MonoBehaviour {
        
        [Header("Spawing Variables.")]
        [SerializeField] private LayerMask ignoreLayer;
        [SerializeField] private LayerMask enemyLayer;
        [SerializeField] private int spawnNumber = 2;
        [SerializeField] private Vector3 size;
        [SerializeField] private Vector3 center;
        
        [SerializeField] private EnemyController enemyPrefab;
        [SerializeField] private List<Vector3> spawnPointList;
        [SerializeField] private bool ranomiseEnemyCount;
        private List<EnemyController> enemiesList;

        private void Awake(){
            if(ranomiseEnemyCount){
                spawnNumber = Random.Range(1,7);
            }
            FindSpawnPoints();
            SpawnObjects();
        }
        
        
        private void FindSpawnPoints(){
            spawnPointList = new List<Vector3>();
            for(int i = 0; i < spawnNumber; i++){
                Vector3 point = center + new Vector3(Random.Range(-size.x/2f,size.z/2f),0f,Random.Range(-size.z/2f,size.z/2f));
                Ray ray = new Ray(point,Vector3.down);
                if(Physics.Raycast(ray,out RaycastHit hit,float.MaxValue,ignoreLayer)){
                    Vector3 setPoint = hit.point;
                    if(Physics.CheckSphere(setPoint,2f,enemyLayer)){
                        setPoint = transform.right + setPoint;
                    }else{
                        setPoint = hit.point;
                    }
                    if(!spawnPointList.Contains(setPoint)){
                        spawnPointList.Add(setPoint);
                    }
                    
                }
            }
            
        }

        public List<EnemyController> GetEnemieList(){
            return enemiesList;
        }

        
        private void SpawnObjects(){
            enemiesList = new List<EnemyController>();
            for (int i = 0; i < spawnPointList.Count; i++){
                EnemyController newEnemy = Instantiate(enemyPrefab,spawnPointList[i],Quaternion.identity);
                newEnemy.transform.SetParent(transform);
                if(!enemiesList.Contains(newEnemy)){
                    enemiesList.Add(newEnemy);
                }
                
            }
        }
        private void OnDrawGizmos(){
            Gizmos.color = Color.red;
            Gizmos.DrawCube(transform.position + center,size);
        }
    }

}