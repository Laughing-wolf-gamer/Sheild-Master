using UnityEngine;
using GamerWolf.Utils.HealthSystem;

namespace SheildMaster {
    public class PlayerAnimation : MonoBehaviour {
        
        
        [SerializeField] private HealthEntity player;
        private int randomDeathAnim,randomJoyAnim;
        private Animator animator;

        private bool isWon;        
        private void Awake(){
            animator = GetComponent<Animator>();
        }
        private void Start(){
            
            isWon = false;
            randomDeathAnim =Random.Range(0,3);
            randomJoyAnim = Random.Range(0,2);
            GameHandler.current.onGameOver += OnGameOver;
            player.OnHit += OnPlayerDeath;
        }
        
        private void OnPlayerDeath(object sender,System.EventArgs e){
            PlayIsDeadAnimations();
            
        }
        private void OnGameOver(object sender , OnGamoverEventsAargs e){
            animator.applyRootMotion = true;
            if(e.iswin){
                PlayIsWonAnimations();
            }else{
                AudioManager.current.PlayOneShotMusic(SoundType.Player_Death);
                PlayIsDeadAnimations();
            }
        }
        public void PlayIsWonAnimations(){
            if(!isWon){
                animator.SetInteger("dance Number",randomJoyAnim);
                animator.SetTrigger("isWon");
                isWon = true;
            }
            
        }
        public void PlayIsDeadAnimations(){
            animator.SetInteger("death Numb",randomDeathAnim);
            animator.SetTrigger("isDead");
            
        }
    }

}