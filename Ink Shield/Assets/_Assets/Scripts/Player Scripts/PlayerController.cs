using System;
using UnityEngine;
using GamerWolf.Utils.HealthSystem;
namespace InkShield {
    public class PlayerController : HealthEntity{
        
        
        [Header("External References")]
        [SerializeField] private PlayerDataSO playerData;
        [SerializeField] private GameHandler gameHandler;
        [SerializeField] private PlayerAnimation playerAnimation;
        [Header("Inputs Toggle")]
        [SerializeField] private bool onPc = true;
        [Space(20)]
        [SerializeField] private Transform targetPoint;
        [SerializeField] private GameObject forceFieldObject;
        [SerializeField] private Transform onWinLookPoint;
        

        #region Private Variables....
        private bool enableInpts;
        private PlayerInputController playerInputController;
        private WallCreater wallCreater;
        #endregion

        #region Singelton...
        public static PlayerController player;
        protected override void Awake(){
            base.Awake();
            if(player == null){
                player = this;
            }else{
                Destroy(player.gameObject);
            }
            wallCreater = GetComponent<WallCreater>();
            playerInputController = GetComponent<PlayerInputController>();
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
                if(onPc){
                    playerInputController.GetPcInput();
                }else{
                    playerInputController.GetMobileInputs();
                }
                wallCreater.TryDrawWall();
                RotatePlayer(targetPoint.position);
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
        
        public int GetTouchCount(){
            return wallCreater.GetTouchCount();
        }
        
        public void OnLevelComplete(){
            playerData.OnLevelComplete();
        }
        public void ActivateForceField(){
            forceFieldObject.SetActive(true);
        }
        
    }

}