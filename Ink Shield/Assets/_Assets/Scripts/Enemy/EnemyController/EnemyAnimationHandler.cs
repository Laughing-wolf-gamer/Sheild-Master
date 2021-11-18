using UnityEngine;
using GamerWolf.Utils;
using UnityEngine.Animations.Rigging;
namespace InkShield {
    [RequireComponent(typeof(RagDollManagment))]
    public class EnemyAnimationHandler : MonoBehaviour {
        

        private Animator animator;
        [SerializeField] private Rig rig;

        private void Awake(){
            animator = GetComponent<Animator>();
        }
        private void Start(){
            GameHandler.current.onGameOver += OnGameOver;
        }
        private void OnGameOver(object sender , OnGamoverEventsAargs e){
            rig.weight = 0f;
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
        }
        public void PlayShootingAnimations(){
            rig.weight = 1f;
            animator.SetTrigger("Shoot");
        }
        
    }
}
