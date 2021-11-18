using UnityEngine;
using GamerWolf.Utils;
using System.Collections;
namespace InkShield {
    public class Wall : MonoBehaviour,IPooledObject{
        
        
        [SerializeField] private float lifeTime = 3f;
        [SerializeField] private Animator wallAnimator;
        private Vector3 targetToLook;
        private BoxCollider thisCollider;
        private void Awake(){
            thisCollider = GetComponent<BoxCollider>();
        }
        
        public void DestroyMySelf(){
            wallAnimator.SetBool("Grow",false);
        }
        public void HideWall(){
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
            wallAnimator.SetBool("Grow",true);
            RotateToWardsTarget();
        }
        private void RotateToWardsTarget(){
            Vector3 dir = (targetToLook - transform.position).normalized;
            float angle = Mathf.Atan2(dir.x,dir.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle,Vector3.up);
        }


        
        
        
    }

}