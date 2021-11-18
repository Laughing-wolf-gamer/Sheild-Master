using System;
using UnityEngine;
using GamerWolf.Utils.HealthSystem;
namespace InkShield {
    public class PlayerController : HealthEntity{
        
        
        [Header("External References")]
        [Space(20)]
        [SerializeField] private Transform targetPoint;
        [SerializeField] private Transform onWinLookPoint;
        [Space(20)]
        [SerializeField] private GameHandler gameHandler;
        [SerializeField] private PlayerAnimation playerAnimation;

        #region Private Variables....
        private bool enableInpts;
        private PlayerInputController playerInputController;
        private WallCreater wallCreater;
        #endregion

        #region Singelton...
        public static PlayerController player;
        protected override void Awake(){
            if(player == null){
                player = this;
            }else{
                Destroy(player.gameObject);
            }
            wallCreater = GetComponent<WallCreater>();
            playerInputController = GetComponent<PlayerInputController>();
            gameHandler.SetIsPlayerDead(GetIsDead());
            
        }
        protected override void Start(){
            base.Start();
            gameHandler.onGameOver += (object sender, OnGamoverEventsAargs args) =>{
                RotatePlayer(onWinLookPoint.position);
            };
        }
        #endregion

        private void Update(){
            if(enableInpts){
                wallCreater.TryDrawWall();
                RotatePlayer(targetPoint.position);
                #if UNITY_EDITOR
                    playerInputController.GetPcInput();
                #else
                    playerInputController.GetMobileInput();
                #endif
            }
        }

        public void ActivateInputs(bool _value){
            enableInpts = _value;
        }
        private void RotatePlayer(Vector3 target){
            Vector3 dir = (target - transform.position).normalized;
            float angle = Mathf.Atan2(dir.x,dir.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle,Vector3.up);
        }
        public void FillInk(){
            base.ResetHealth();
            wallCreater.FillInk(20);
        }


        
    }

}