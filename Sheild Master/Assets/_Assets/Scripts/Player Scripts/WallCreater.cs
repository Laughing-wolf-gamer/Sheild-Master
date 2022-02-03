using System;
using UnityEngine;
using GamerWolf.Utils;
using UnityEngine.Animations.Rigging;
namespace SheildMaster {

    public class WallCreater : MonoBehaviour {


        #region Exposed Variables...
        [Header("Ink Settings")]
        [SerializeField] private PlayerDataSO playerData;
        [SerializeField] private int maxInkAmount = 100;

        [Header("Mouse Targets Variables")]
        [SerializeField] private Transform mouseObject;
        [SerializeField] private float maxDistanceForSpawn = 2f;

        [Header("Animation Rigging")]
        [SerializeField] private Rig rig;

        #endregion

        #region Private Variables.....
        private ObjectPoolingManager objectPoolingManager;
        private PlayerInputController playerInputController;
        private int currentInkAmount;
        private Vector3 previousMousePositon;
        private ExpandingWall currentWall;
        private Vector3 previousDir;
        private Vector3 initPoint;
        private float firstTouchDistance;
        private const float maxDistanceFromPlayer = 13f;
        #endregion


        #region System Events.....
        public Action onInkChange;

        #endregion



        private void Awake(){
            playerInputController = GetComponent<PlayerInputController>();
        }
        private void Start(){
            SetInkAmount();
            objectPoolingManager = ObjectPoolingManager.current;
            onInkChange += ()=>{
                UIHandler.current.SetInkTankValue(GetInkNormalizedValue());
            };
            GameHandler.current.onGameOver += (object sender,OnGamoverEventsAargs e)=>{
                rig.weight = 0f;
            };
        }
        private void SetInkAmount(){
            if(playerData.GetLevelNumber() <= 25){
                maxInkAmount = 10;
            }
            if(playerData.GetLevelNumber() > 25){
                maxInkAmount = 20;
            }
            if(playerData.GetLevelNumber() >= 60){                
                maxInkAmount = 40;
            }
            currentInkAmount = maxInkAmount;
        }
        
        
        public void TryDrawWall(){
            if(currentInkAmount > 0){
                mouseObject.position = playerInputController.GetMousePoint();
                if(playerInputController.GetTouchStarted()){
                    // Check if the Player is Touching the screen Very Far.
                    firstTouchDistance = Vector3.Distance(transform.position,playerInputController.GetMousePoint());
                    // Set First Point.
                    initPoint = mouseObject.position;
                    if(firstTouchDistance <= maxDistanceFromPlayer){
                        rig.weight = 1f;
                        ReduceInk();
                        currentWall = SpawnNewWall(mouseObject.position);
                        previousDir = (initPoint - mouseObject.position).normalized;
                    }else{
                        UIHandler.current.ShowErrorWindow();
                    }
                }
                if(playerInputController.GetTouchMoving()){
                    if(firstTouchDistance <= maxDistanceFromPlayer){
                        Vector3 currentDir = (previousDir - mouseObject.position).normalized;
                        currentWall.SetExpandDir(mouseObject.position);
                        if(previousDir != currentDir){
                            previousDir = currentDir;
                            if(Vector3.Distance(initPoint,mouseObject.position) >= maxDistanceForSpawn){
                                
                                ExpandingWall wall = SpawnNewWall(currentWall.GetNewWallSpawnPoint());
                                if(wall != currentWall){
                                    currentWall = wall;
                                }
                                initPoint = mouseObject.position;
                                ReduceInk();
                            }
                        }
                    }
                }
            }
            if(playerInputController.GetTouchEnded()){
                rig.weight = 0f;
            }
            
        }
        private ExpandingWall SpawnNewWall(Vector3 pos){
            GameObject obj = objectPoolingManager.SpawnFromPool(PoolObjectTag.Wall,pos,Quaternion.identity);
            ExpandingWall wall = obj.GetComponent<ExpandingWall>();
            if(wall != null){
                return wall;
            }
            return null;
        }
        
        
        
        public void ReduceInk(){
            currentInkAmount--;
            if(currentInkAmount <= 0){
                currentInkAmount = 0;
            }
            onInkChange?.Invoke();
        }
        public void FillInk(int _value){
            if(currentInkAmount <= 0){
                currentInkAmount += _value;
            }
            onInkChange?.Invoke();
        }
        public float GetInkNormalizedValue(){
            return currentInkAmount/(float)maxInkAmount;
        }
        
       
        
    }

}