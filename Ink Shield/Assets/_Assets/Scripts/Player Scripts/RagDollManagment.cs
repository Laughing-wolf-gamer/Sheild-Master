using UnityEngine;

namespace GamerWolf.Utils{
    [RequireComponent(typeof(Animator))]
    public class RagDollManagment : MonoBehaviour {

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
            mainRigidBody.AddForce(Vector3.up * 20f ,ForceMode.Impulse);
            mainRigidBody.AddForce(transform.forward * -10f,ForceMode.Impulse);
            ActivateRagdoll(true);
        }
        private void ActivateRagdoll(bool activate){
            if(activate){
                animator.enabled = false;
                mainCollider.enabled = false;
                mainRigidBody.isKinematic = true;
                foreach(Collider collis in bodyCollidersArray){
                    collis.enabled = true;   
                }
                foreach(Rigidbody rbs in rigidbodiesArray){
                    rbs.isKinematic = false;
                }
            }else{
                mainCollider.enabled = true;
                mainRigidBody.isKinematic = false;
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