using System;
using UnityEngine;
using GamerWolf.Utils;
using System.Collections;
using GamerWolf.Utils.HealthSystem;

namespace SheildMaster {
    public enum EnemyType{
        Normal,Armourd,Super
    }
    
    public class EnemyController : HealthEntity {
        
        [SerializeField] protected EnemyType enemyType;

        [Header("Enemy Shooting")]
        [SerializeField] private GameObject wepon;
        [SerializeField] private Transform firePoint;
        [SerializeField] private float maxFireTime = 4f;

        [Header("External References")]
        [SerializeField] private EnemyAnimationHandler animationHandler;
        [SerializeField] private Rotator[] rotators;
        
        private ObjectPoolingManager objectPoolingManager;
        private PlayerController player;

        [Header("Initial Check")]
        [SerializeField] private float checkRange = 2f;
        [SerializeField] private LayerMask checkLayer;
        
        protected override void Awake(){
            base.Awake();
        }
        protected override void Start(){
            // MultiTargetCameraController.current.SetTargetToList(this.transform);
            SetRootMotion(false);
            base.OnHit += (object sender ,EventArgs e) => {
                GameHandler.current.IncreaseKills();
            };
   
            base.Start();
            player = PlayerController.player;
            objectPoolingManager = ObjectPoolingManager.current;
            base.onDead += (object sender,EventArgs e) =>{
                animationHandler.PlayIsDeadAnimations();
                StopCoroutine(nameof(ShootingRoutine));
                // MultiTargetCameraController.current.RemoveTarget(this.transform);
                SetRootMotion(true);
            };
            RotateTowardsPlayer();
        }
        
        public void StartEnemy(){
            StartCoroutine(nameof(ShootingRoutine));
            GameHandler.current.onGameOver += (object sender,OnGamoverEventsAargs e)=> {
                StopCoroutine(nameof(ShootingRoutine));
                wepon.SetActive(false);
            };
        }

        
        
        private IEnumerator ShootingRoutine(){
            yield return new WaitForSeconds(1f);
            while(!isDead){
                Fire();
                RotateTowardsPlayer();
                yield return new WaitForSeconds(maxFireTime);
            }
        }
        
        public void EndGame(){
            StopCoroutine(nameof(ShootingRoutine));
        }
        // private void Update(){
        //     // if(!isDead){
        //     //     RotateTowardsPlayer();
        //     // }
        // }
        private bool hasWallinFront(){
            if(Physics.Raycast(transform.position,transform.forward,out RaycastHit hit,checkRange,checkLayer,QueryTriggerInteraction.UseGlobal)){
                Debug.Log("hit with " + hit.transform.name);
                return true;
            }
            return false;
        }

        protected virtual void Fire(){
            if(!hasWallinFront()){
                wepon.SetActive(true);
                GameObject projectile =  objectPoolingManager.SpawnFromPool(PoolObjectTag.Projectile,firePoint.position,firePoint.rotation);
                Projectile bullet = projectile.GetComponent<Projectile>();
                if(bullet != null){
                    bullet.SetCameFromEnemy(this);
                }
                animationHandler.PlayShootingAnimations();
                AudioManager.current.PlayMusic(SoundType.Fire_Sound);
            }else {
                Debug.Log("Wall infront");
                TakeHit(20);
            }
        }

        public void SetRootMotion(bool _value){
            animationHandler.GetComponent<Animator>().applyRootMotion = _value;
        }
        
        private void RotateTowardsPlayer(){
            for (int i = 0; i < rotators.Length; i++){
                rotators[i].Rotate(player.transform);
            }
        }
        
        public EnemyType GetEnemyType(){
            return enemyType;
        }
        private void OnDrawGizmos(){
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position,transform.forward * checkRange);
        }
        
    }

}