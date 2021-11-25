using UnityEngine;

namespace GamerWolf.Utils{
    [RequireComponent(typeof(Animator))]
    public class RagDollManagment : MonoBehaviour {
        [SerializeField] private bool canRagDoll;
        [SerializeField] private Collider mainCollider;
        [SerializeField] private Rigidbody mainRigidBody;
        [SerializeField] private Collider[] bodyCollidersArray;
        [SerializeField] private Rigidbody[] rigidbodiesArray;

        private Animator animator;
        private void Awake(){
            animator = GetComponent<Animator>();
        }
        private void Start(){
            ActivateRagdoll(false);

        }
        public void StartRagDoll(){
            if(canRagDoll){
                ActivateRagdoll(true);
            }
        }
        private void ActivateRagdoll(bool activate){
            if(activate){
                animator.enabled = false;
                if(mainCollider != null){
                    mainCollider.enabled = false;
                }
                if(mainRigidBody != null){
                    mainRigidBody.isKinematic = true;
                }
                foreach(Collider collis in bodyCollidersArray){
                    collis.enabled = true;   
                }
                foreach(Rigidbody rbs in rigidbodiesArray){
                    rbs.isKinematic = false;
                }
            }else{
                if(mainCollider != null){
                    mainCollider.enabled = true;

                }
                if(mainRigidBody != null){
                    mainRigidBody.isKinematic = false;
                }
                foreach(Collider collis in bodyCollidersArray){
                    collis.enabled = false;   
                }
                foreach(Rigidbody rbs in rigidbodiesArray){
                    rbs.isKinematic = true;
                }
            }
        }
        
    }

}