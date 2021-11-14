using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace InkShield{ 
    public class LevelData : MonoBehaviour {
        
        [SerializeField] private List<EnemyController> enemiesList;
        


        public List<EnemyController> GetEnemieList(){
            return enemiesList;
        }
    }

}