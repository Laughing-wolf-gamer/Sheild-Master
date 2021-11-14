using UnityEngine;
using GamerWolf.Utils;
using System.Collections;
namespace InkShield {
    public class Wall : MonoBehaviour,IPooledObject{
        
        
        [SerializeField] private float lifeTime = 3f;
        
        private Vector3 targetToLook;
        private BoxCollider thisCollider;
        private void Awake(){
            thisCollider = GetComponent<BoxCollider>();
        }
        
        public void DestroyMySelf(){
            targetToLook = Vector3.zero;
            thisCollider.enabled = false;
            gameObject.SetActive(false);
        }
        

        public void OnObjectReuse(){
            thisCollider.enabled = true;
            Invoke(nameof(DestroyMySelf),lifeTime);
        }
        public void SetNextWall(Transform lookAtTarget){
            targetToLook = lookAtTarget.position;
            // Debug.Log(" Wall "+ transform.name + "looking to " + lookAtTarget.name);
            RotateToWardsTarget();
        }
        private void RotateToWardsTarget(){
            Vector3 dir = (targetToLook - transform.position).normalized;
            float angle = Mathf.Atan2(dir.x,dir.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle,Vector3.up);
        }


        
        
        
    }

}