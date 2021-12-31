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
        [SerializeField] private HealthBar healthBar;
        [SerializeField] private Rotator[] rotators;
        
        private ObjectPoolingManager objectPoolingManager;
        private PlayerController player;
        
        protected override void Awake(){
            base.Awake();
        }
        protected override void Start(){
            
            base.OnHit += (object sender ,EventArgs e) => {
                healthBar.UpdateHealthBar(base.GetHealthNormalized());
                GameHandler.current.IncreaseKills();
            };
            healthBar.HideHealthBar();
            base.Start();
            player = PlayerController.player;
            objectPoolingManager = ObjectPoolingManager.current;
            base.onDead += (object sender,EventArgs e) =>{
                animationHandler.PlayIsDeadAnimations();
                StopCoroutine(nameof(ShootingRoutine));
            };
        }
        
        public void StartEnemy(){
            StartCoroutine(nameof(ShootingRoutine));
            GameHandler.current.onGameOver += (object sender,OnGamoverEventsAargs e)=> {
                StopCoroutine(nameof(ShootingRoutine));
                wepon.SetActive(false);
            };
        }
        public void SetViewCameraForHealthBar(Camera viewCamera){
            healthBar.SetworldCamera(viewCamera);
        }
        
        
        private IEnumerator ShootingRoutine(){
            yield return new WaitForSeconds(1f);
            while(!isDead){
                Fire();
                
                yield return new WaitForSeconds(maxFireTime);
            }
        }
        
        public void EndGame(){
            StopCoroutine(nameof(ShootingRoutine));
        }
        private void Update(){
            if(!isDead){
                RotateTowardsPlayer();
            }
        }
        

        protected virtual void Fire(){
            
            wepon.SetActive(true);
            GameObject projectile =  objectPoolingManager.SpawnFromPool(PoolObjectTag.Projectile,firePoint.position,firePoint.rotation);
            Projectile bullet = projectile.GetComponent<Projectile>();
            if(bullet != null){
                bullet.SetCameFromEnemy(this);
            }
            animationHandler.PlayShootingAnimations();
            AudioManager.current.PlayMusic(SoundType.Fire_Sound);
        }

        
        
        private void RotateTowardsPlayer(){
            for (int i = 0; i < rotators.Length; i++){
                rotators[i].Rotate(player.transform);
            }
        }
        
        public EnemyType GetEnemyType(){
            return enemyType;
        }
        
        
    }

}