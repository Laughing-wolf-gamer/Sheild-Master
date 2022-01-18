using UnityEngine;
namespace SheildMaster {
    public class PlayerAbilitySystem : MonoBehaviour {
        [SerializeField] private AbilitySO ability1,ability2;
        
        public void IncreaseKillOneEnemyAbiliy(int amount){
            ability1.IncreaseAbility(amount);
        }
        public void IncreaseOne_Bullet_Armour(int amount){
            ability2.IncreaseAbility(amount);
        }
    }
    

}