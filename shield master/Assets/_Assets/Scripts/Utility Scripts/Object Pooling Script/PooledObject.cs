using UnityEngine;
using UnityEngine.Events;
namespace GamerWolf.Utils {
    public class PooledObject : MonoBehaviour, IPooledObject{
        [SerializeField] private UnityEvent onObjectUse,onObjectDestroy;
        
        public void DestroyMySelf(float delay = 0f){
            onObjectDestroy?.Invoke();
            Invoke(nameof(DestroyWithDelay),delay);
            
        }
        private void DestroyWithDelay(){
            gameObject.SetActive(false);
        }

        public void OnObjectReuse(){
            onObjectUse?.Invoke();
        }
    }

}