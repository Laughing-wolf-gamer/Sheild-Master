using UnityEngine;
using GamerWolf.Utils;
using UnityEngine.Events;
using GamerWolf.Utils.HealthSystem;
namespace SheildMaster {
    public class Projectile : MonoBehaviour, IPooledObject {


        [Header("Events")]
        [SerializeField] private UnityEvent onResuse;
        [SerializeField] private UnityEvent onDestroy;
        [Space(20)]
        [Header("Movement Variables")]
        [SerializeField] private LayerMask collisonMask;
        [SerializeField] private float moveForce = 20f;
        [SerializeField] private float maxLifeTime = 5f;
        private EnemyController cameFromEnemy;
        private bool Move;
        private int collisionCount;
        private Collider m_collider;
        private AudioManager audioManager;
        private void Start(){
            audioManager = AudioManager.current;
            PlayerController.player.onDead += (object sender,System.EventArgs e) =>{
                DestroyMySelf();
            };
            cameFromEnemy.onDead += (object sender,System.EventArgs e) =>{
                DestroyMySelf();
            };
            GameHandler.current.onGameOver += (object sender , OnGamoverEventsAargs args) =>{
                DestroyMySelf();
            };
        }
        
        public void SetCameFromEnemy(EnemyController enemyController){
            cameFromEnemy = enemyController;
            Move = true;
        }


        public void DestroyMySelf(){
            onDestroy?.Invoke();
            Move = false;
            cameFromEnemy = null;
            gameObject.SetActive(false);
            collisionCount = 0;
        }

        public void OnObjectReuse(){
            collisionCount = 0;
            Invoke(nameof(DestroyMySelf),maxLifeTime);
            onResuse?.Invoke();
        }
        
        private void Update(){
            if(Move){
                Movement();
            }

        }
        private void Movement(){
            float moveDista = moveForce * Time.deltaTime;
            CheckCollision(moveDista);
            transform.Translate(Vector3.forward * moveDista);

        }
        private void CheckCollision(float moveDistance){
            Ray ray = new Ray (transform.position,transform.forward);
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit,moveDistance,collisonMask,QueryTriggerInteraction.Collide)){
                OnHitObject(hit);
            }
        }
        private void OnHitObject(RaycastHit _hit){
            collisionCount++;
            audioManager.PlayOneShotMusic(SoundType.Wall_hit);
            if(_hit.transform.TryGetComponent<IDamagable>(out IDamagable damagable)){
                damagable.TakeHit(1);
                DestroyMySelf();
            }else{
                if(_hit.transform.TryGetComponent<ForcefieldImpact>(out ForcefieldImpact forcefield)){
                    forcefield.OnImpcat(_hit);
                }
                // if(cameFromEnemy.GetEnemyType() == EnemyType.Armourd || cameFromEnemy.GetEnemyType() == EnemyType.Super){
                //     if(_hit.transform.TryGetComponent<ExpandingWall>(out ExpandingWall wall)){
                //         if(collisionCount <= 1){
                //             moveForce *= 0.5f;
                //             wall.DestroyMySelf();
                //             Movement();
                //         }else{
                //             if(cameFromEnemy != null){
                //                 transform.LookAt(cameFromEnemy.transform);
                //             }else{
                //                 DestroyMySelf();
                //             }
                //         }
                //     }
                // }else{
                // }
                if(cameFromEnemy != null){
                    transform.LookAt(cameFromEnemy.transform);
                }else{
                    DestroyMySelf();
                }
            }
        }
        
        
        
        
    }

}