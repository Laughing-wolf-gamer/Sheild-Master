using System;
using UnityEngine;
using GamerWolf.Utils;
using System.Collections;
using GamerWolf.Utils.HealthSystem;

namespace InkShield {
    
    public class EnemyController : HealthEntity {
        
        [SerializeField] private float cuttOffValue = 8f;
        [SerializeField] private Transform firePoint;
        [SerializeField] private GameObject wepon;
        [SerializeField] private LayerMask wallLayer;
        [SerializeField] private Rotator[] rotators;
        [SerializeField] private EnemyAnimationHandler animationHandler;
        
        [SerializeField] private float maxFireTime = 4f;
        private ObjectPoolingManager objectPoolingManager;
        private PlayerController player;
        private Rigidbody rb;
        private string timerName = "Fire Timer";
        private bool isPlayerDead;
        private bool startDessolve;
        protected override void Awake(){
            base.Awake();
            rb = GetComponent<Rigidbody>();

        }
        
        protected override void Start(){
            base.Start();
            player = PlayerController.player;
            objectPoolingManager = ObjectPoolingManager.current;
            
            
        }
        public void StartEnemy(){
            int rand = UnityEngine.Random.Range(0,5);
            if(rand > 3){
                rb.AddForce((Vector3.up) * 2f,ForceMode.Impulse);
            }
            Debug.Log("On Game Start");
            StartCoroutine(nameof(ShootingRoutine));
            base.onDead += (object sender,EventArgs e) =>{
                animationHandler.PlayIsDeadAnimations();
                StopCoroutine(nameof(ShootingRoutine));
            };
            GameHandler.current.onGameOver += (object sender,OnGamoverEventsAargs e)=> {
                StopCoroutine(nameof(ShootingRoutine));
                wepon.SetActive(false);
            };
        }
        
        
        private IEnumerator ShootingRoutine(){
            yield return new WaitForSeconds(2f);
            while(!isDead){
                if(!isPlayerDead){
                    Fire();
                }
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
        

        private void Fire(){
            if(!Physics.Raycast(transform.position,transform.forward,5f,wallLayer)){
                wepon.SetActive(true);
                GameObject projectile =  objectPoolingManager.SpawnFromPool(PoolObjectTag.Projectile,firePoint.position,firePoint.rotation);
                Projectile bullet = projectile.GetComponent<Projectile>();
                if(bullet != null){
                    bullet.SetCameFromEnemy(this);
                }
                animationHandler.PlayShootingAnimations();
            }else{
                TakeHit(4);
            }
        }
        
        private void RotateTowardsPlayer(){
            for (int i = 0; i < rotators.Length; i++){
                rotators[i].Rotate(player.transform);
            }
        }
        
        
        public void PlayerDead(bool value){
            if(!isDead){
                isPlayerDead = value;
            }
            
        }
        
    }

}