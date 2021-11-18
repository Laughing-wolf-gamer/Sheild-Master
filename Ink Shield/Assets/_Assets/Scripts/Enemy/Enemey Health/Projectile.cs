using UnityEngine;
using GamerWolf.Utils;
using UnityEngine.Events;
using GamerWolf.Utils.HealthSystem;
namespace InkShield {
    public class Projectile : MonoBehaviour, IPooledObject {


        [Header("Events")]
        [SerializeField] private UnityEvent onResuse;
        [SerializeField] private UnityEvent onDestroy;
        
        [Space(20)]

        [Header("Movement Variables")]
        [SerializeField] private float moveForce = 20f;
        [SerializeField] private float maxLifeTime = 5f;
        private EnemyController cameFromEnemy;
        private Rigidbody rb;
        private void Awake(){
            rb = GetComponent<Rigidbody>();
        }
        private void Start(){
            GameHandler.current.onGameOver += (object sender , OnGamoverEventsAargs args) =>{
                DestroyMySelf();
            };
        }
        
        public void SetCameFromEnemy(EnemyController enemyController){
            cameFromEnemy = enemyController;
        }


        public void DestroyMySelf(){
            gameObject.SetActive(false);
        }

        public void OnObjectReuse(){
            Move();
            Invoke(nameof(DestroyMySelf),maxLifeTime);
        }
        
        private void Move(){
            rb.AddForce(transform.forward * moveForce,ForceMode.Impulse);
        }
        private void OnCollisionEnter(Collision colli){
            rb.velocity = Vector3.zero;
            if(colli.gameObject.TryGetComponent<IDamagable>(out IDamagable damagable)){
                damagable.TakeHit(5);
                DestroyMySelf();
            }else{
                rb.velocity = Vector3.zero;
                transform.LookAt(cameFromEnemy.transform);
                Move();
            }
        }
        
        
    }

}