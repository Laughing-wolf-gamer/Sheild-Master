using UnityEngine;
using GamerWolf.Utils;
using System.Collections;
namespace SheildMaster { 
    public class ExpandingWall : MonoBehaviour,IPooledObject{
        [SerializeField] private Transform newWallSpawnPoint;
        [SerializeField] private float lifeTime = 4f;
        private Vector3 dirToExpanding;
        private float amountToExpanding;
        [SerializeField]private BoxCollider m_collider;
        [SerializeField] private Animator animator;

        private void Start(){
            GameHandler.current.onGameOver += (object sender,OnGamoverEventsAargs e) =>{
                DestroyMySelf();
            };
        }
        public void DestroyMySelf(float delay = 0f){
            m_collider.enabled = false;
            animator.SetBool("Grow",false);
            CancelInvoke(nameof(HideWall));
            Invoke(nameof(HideWall),delay);
        }
        public void HideWall(){
            gameObject.SetActive(false);
            StopAllCoroutines();
        }
        public void OnObjectReuse(){
            m_collider.enabled = true;
            animator.SetBool("Grow",true);
            StartCoroutine(DestroyRoutine(lifeTime));
        }
        private IEnumerator DestroyRoutine(float lifeTime){
            yield return new WaitForSeconds(lifeTime);
            DestroyMySelf();
        }

        public void SetExpandDir(Vector3 dir){
            amountToExpanding = Vector3.Distance(transform.position,dir);
            transform.localScale = new Vector3(transform.localScale.x,transform.localScale.y,amountToExpanding);
            RotateToWardsTarget(dir);
        }
        private void RotateToWardsTarget(Vector3 targetPos){
            Vector3 dir = (targetPos - transform.position).normalized;
            float angle = Mathf.Atan2(dir.x,dir.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle,Vector3.up);
        }
        public Vector3 GetNewWallSpawnPoint(){
            return newWallSpawnPoint.position;
        }
    }

}