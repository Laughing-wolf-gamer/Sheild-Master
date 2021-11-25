using UnityEngine;
namespace InkShield {
    public class PlayerAbilitySystem : MonoBehaviour {
        [SerializeField] private AbilitySO ability1,ability2;
        
        public void IncreaseKillOneEnemyAbiliy(){
            ability1.IncreaseAbiliyValue(5);
        }
        public void IncreaseOne_Bullet_Armour(){
            ability2.IncreaseAbiliyValue(2);
        }
    }
    

}