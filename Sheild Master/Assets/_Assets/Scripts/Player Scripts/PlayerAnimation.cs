using UnityEngine;
using GamerWolf.Utils.HealthSystem;

namespace SheildMaster {
    public class PlayerAnimation : MonoBehaviour {
        
        
        private Animator animator;
        [SerializeField] private HealthEntity player;
        private int randomDeathAnim,randomJoyAnim;

        private void Awake(){
            animator = GetComponent<Animator>();
        }
        private void Start(){
            randomDeathAnim =Random.Range(0,3);
            randomJoyAnim = Random.Range(1,3);
            GameHandler.current.onGameOver += OnGameOver;
            player.OnHit += OnPlayerDeath;
        }
        
        private void OnPlayerDeath(object sender,System.EventArgs e){
            PlayIsDeadAnimations();
        }
        private void OnGameOver(object sender , OnGamoverEventsAargs e){
            if(e.iswin){
                PlayIsWonAnimations();
            }else{
                PlayIsDeadAnimations();
            }
        }
        public void PlayIsWonAnimations(){
            
            animator.SetTrigger("isWon");
            animator.SetInteger("dance Number",randomJoyAnim);
            
        }
        public void PlayIsDeadAnimations(){
            
            animator.SetInteger("death Numb",randomDeathAnim);
            animator.SetTrigger("isDead");
        }
    }

}