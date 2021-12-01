using UnityEngine;


namespace GamerWolf.Utils.HealthSystem {
    public interface IDamagable {
        
        void TakeHit(int damageValue);
        void ResetHealth();
    }

}