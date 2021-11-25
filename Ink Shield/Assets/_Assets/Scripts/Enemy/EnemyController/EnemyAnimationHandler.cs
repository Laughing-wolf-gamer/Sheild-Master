using UnityEngine;
using GamerWolf.Utils;
using UnityEngine.Animations.Rigging;
namespace InkShield {
    public class EnemyAnimationHandler : MonoBehaviour {
        

        [SerializeField] private Rig rig;
        private Animator animator;

        private void Awake(){
            animator = GetComponent<Animator>();
        }
        private void Start(){
            GameHandler.current.onGameOver += OnGameOver;
        }
        private void OnGameOver(object sender , OnGamoverEventsAargs e){
            StopRig();
            if(e.iswin){
                PlayIsDeadAnimations();
            }else{
                PlayIsWonAnimations();
            }
        }
        private void PlayIsWonAnimations(){
            animator.SetTrigger("isWon");
            int rand = UnityEngine.Random.Range(1,3);
            animator.SetInteger("dance Number",rand);
        }
        public void PlayIsDeadAnimations(){

            int rand = Random.Range(0,4);
            animator.SetInteger("death Numb",rand);
            animator.SetTrigger("isDead");
            rig.weight = 0f;
        }
        public void PlayShootingAnimations(){
            rig.weight = Mathf.Lerp(rig.weight,1f,1f);
            animator.SetTrigger("Shoot");
        }
        public void StopRig(){
            rig.weight = Mathf.Lerp(rig.weight,0f,1f);
            
        }
        
    }
}
