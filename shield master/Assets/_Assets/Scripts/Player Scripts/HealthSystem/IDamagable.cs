using UnityEngine;


namespace GamerWolf.Utils.HealthSystem {
    public interface IDamagable {
        
        void TakeHit(int damageValue);
        void TakeHit(int damageValue,Vector3 hitPoint);
        void ResetHealth();
    }

}