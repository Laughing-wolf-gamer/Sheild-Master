using UnityEngine;
using GamerWolf.Utils;

namespace SheildMaster {
    public class PlayerAnimation : MonoBehaviour {
        
        
        [SerializeField] private PlayerController player;
        private Animator animator;

        private void Awake(){
            animator = GetComponent<Animator>();
        }
        private void Start(){
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
            int rand = Random.Range(0,4);
            animator.SetInteger("dance Number",rand);
            
        }
        public void PlayIsDeadAnimations(){
            int rand = Random.Range(0,4);
            animator.SetInteger("death Numb",rand);
            animator.SetTrigger("isDead");
        }
    }

}