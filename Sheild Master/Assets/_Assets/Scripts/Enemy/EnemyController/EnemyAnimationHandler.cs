using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace SheildMaster {
    public class EnemyAnimationHandler : MonoBehaviour {
        
        
        private int randomJoyAnim,randomDeathAnim;
        private Animator animator;
        [SerializeField] private Rig rig;
        private bool isAlreadyDead;
        private void Awake(){
            animator = GetComponent<Animator>();
        }
        private void Start(){
            isAlreadyDead = false;
            rig.weight = 0f;
            randomJoyAnim = UnityEngine.Random.Range(1,3);
            randomDeathAnim = Random.Range(0,4);
            GameHandler.current.onGameOver += OnGameOver;
        }
        private void OnGameOver(object sender , OnGamoverEventsAargs e){
            StopRig();
            if(e.iswin){
                AudioManager.current.PlayMusic(SoundType.Enemy_Death);
                PlayIsDeadAnimations();
            }else{
                PlayIsWonAnimations();
            }
        }
        private void PlayIsWonAnimations(){
            animator.SetTrigger("isWon");
            animator.SetInteger("dance Number",randomJoyAnim);
        }
        public void PlayIsDeadAnimations(){
            if(!isAlreadyDead){
                animator.SetInteger("death Numb",randomDeathAnim);
                animator.SetTrigger("isDead");
                rig.weight = 0f;
                isAlreadyDead = true;
            }
        }
        public void PlayShootingAnimations(){
            rig.weight = 1f;
            animator.SetTrigger("Shoot");
        }
        public void StopRig(){
            rig.weight = 0f;
            
        }
        
    }
}
