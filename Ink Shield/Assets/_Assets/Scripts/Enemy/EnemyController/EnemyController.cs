using System;
using UnityEngine;
using GamerWolf.Utils;
using GamerWolf.Utils.HealthSystem;

namespace InkShield {
    
    public class EnemyController : HealthEntity {
        
        [SerializeField] private Transform firePoint;
        [SerializeField] private Rotator[] rotators;
        
        [SerializeField] private float maxFireTime = 2f;
        private ObjectPoolingManager objectPoolingManager;
        private PlayerController player;
        private string timerName = "Wepon Timer";

        
        protected override void Start(){
            base.Start();
            player = PlayerController.player;
            objectPoolingManager = ObjectPoolingManager.current;
            
        }
        public void StartGame(){
            TimerTickSystem.CreateTimer(Fire,maxFireTime,timerName);
            base.onDead += (object sender,EventArgs e) =>{
                gameObject.SetActive(false);
                TimerTickSystem.StopTimer(timerName);
            };
        }
        public void EndGame(){
            TimerTickSystem.StopTimer(timerName);
        }
        private void Update(){
            if(!isDead){
                RotateTowardsPlayer();
            }
        }

        private void Fire(){
            GameObject projectile =  objectPoolingManager.SpawnFromPool(PoolObjectTag.Projectile,firePoint.position,firePoint.rotation);
            Projectile bullet = projectile.GetComponent<Projectile>();
            if(bullet != null){
                bullet.SetCameFromEnemy(this);
            }
        }
        private void RotateTowardsPlayer(){
            for (int i = 0; i < rotators.Length; i++){
                rotators[i].Rotate(player.transform);
            }
        }
        

        
    }

}