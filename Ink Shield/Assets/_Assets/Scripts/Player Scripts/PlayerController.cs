using UnityEngine;
using GamerWolf.Utils.HealthSystem;
namespace InkShield {
    public class PlayerController : HealthEntity{
        
        


        #region Private Variables....
        private bool enableInpts;
        private WallCreater wallCreater;
        [SerializeField] private Transform targetPoint;

        #endregion

        #region Singelton...
        public static PlayerController player;
        private void Awake(){
            if(player == null){
                player = this;
            }else{
                Destroy(player.gameObject);
            }
            wallCreater = GetComponent<WallCreater>();
        }
        #endregion

        private void Update(){
            if(enableInpts){
                wallCreater.TryDrawWall();
                RotatePlayer();
            }
        }

        public void ActivateInputs(bool _value){
            enableInpts = _value;
        }
        private void RotatePlayer(){
            Vector3 dir = (targetPoint.position - transform.position).normalized;
            float angle = Mathf.Atan2(dir.x,dir.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle,Vector3.up);
        }
        


        
    }

}