using UnityEngine;
using GamerWolf.Utils;
using UnityEngine.Animations.Rigging;
namespace SheildMaster {
    public class EnemyAnimationHandler : MonoBehaviour {
        
        
        [SerializeField] private ParticleSystem muzzelFlashEffect;
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
            randomDeathAnim = Random.Range(0,3);
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
            if(!isAlreadyDead){
                animator.SetTrigger("isWon");
                animator.SetInteger("dance Number",randomJoyAnim);
            }
        }
        public void PlayIsDeadAnimations(){
            if(!isAlreadyDead){
                animator.SetInteger("death Numb",randomDeathAnim);
                animator.SetTrigger("isDead");
                rig.weight = 0f;
                isAlreadyDead = true;
            }
        }
        public void PlayHitEffect(Vector3 hitPoint,EnemyType enemyType){
            switch(enemyType){

                case EnemyType.Normal:
                    GameObject hitEffectObject = ObjectPoolingManager.current.SpawnFromPool(PoolObjectTag.Hit_Effect_Partical_Red,hitPoint,Quaternion.FromToRotation(Vector3.forward,-hitPoint));
                    hitEffectObject.GetComponent<ParticleSystem>().Play();
                break;

                case EnemyType.Armourd:
                    hitEffectObject = ObjectPoolingManager.current.SpawnFromPool(PoolObjectTag.Hit_Effect_Partical_Green,hitPoint,Quaternion.FromToRotation(Vector3.forward,-hitPoint));
                    hitEffectObject.GetComponent<ParticleSystem>().Play();
                break;

                case EnemyType.Super:
                    hitEffectObject = ObjectPoolingManager.current.SpawnFromPool(PoolObjectTag.Hit_Effect_Partical_Blue,hitPoint,Quaternion.FromToRotation(Vector3.forward,-hitPoint));
                    hitEffectObject.GetComponent<ParticleSystem>().Play();
                break;
                
            }
        }
        public void PlayShootingAnimations(){
            rig.weight = 1f;
            animator.SetTrigger("Shoot");
            muzzelFlashEffect.Play();
        }
        public void StopRig(){
            rig.weight = 0f;
            
        }
        
    }
}
