using System;
using UnityEngine;
using GamerWolf.Utils;
using UnityEngine.Animations.Rigging;
using System.Collections.Generic;
namespace InkShield {

    public class WallCreater : MonoBehaviour {


        #region Exposed Variables...
        [Header("Ink Settings")]
        [SerializeField] private int maxInkAmount = 100;

        [Header("Mouse Targets")]
        [SerializeField] private Transform mouseObject;
        [SerializeField] private float mouseObjectMoveSpeed = 20f;
        [SerializeField] private float maxDistanceForSpawn = 1f;

        [Header("Animation Rigging")]
        [SerializeField] private Rig rig;

        #endregion

        #region Private Variables.....
        private ObjectPoolingManager objectPoolingManager;
        private PlayerInputController playerInputController;
        private int currentInkAmount;
        private Vector3 previousMousePositon;
        private List<Wall> wallList;
        private bool onInkTankEmpty;

        #endregion


        #region System Events.....
        public Action onInkChange;

        #endregion



        private void Awake(){
            playerInputController = GetComponent<PlayerInputController>();
        }
        private void Start(){
            wallList = new List<Wall>();
            currentInkAmount = maxInkAmount;
            objectPoolingManager = ObjectPoolingManager.current;
            onInkChange += ()=>{
                UIHandler.current.SetInkTankValue(GetInkNormalizedValue());
            };
            GameHandler.current.onGameOver += (object sender,OnGamoverEventsAargs e)=>{
                rig.weight = 0f;
            };
        }
       
        public void TryDrawWall(){
            if(currentInkAmount > 0){
                if(playerInputController.GetTouchStarted()){
                    mouseObject.gameObject.SetActive(true);
                    mouseObject.position = Vector3.MoveTowards(mouseObject.position,playerInputController.GetMousePoint(),mouseObjectMoveSpeed * Time.deltaTime);
                    previousMousePositon = mouseObject.position;
                    SpawnWall();
                    ReduceInk(1);
                    rig.weight = 1f;
                }else if(playerInputController.GetTouchMoving()){
                    mouseObject.position = Vector3.MoveTowards(mouseObject.position,playerInputController.GetMousePoint(),mouseObjectMoveSpeed * Time.deltaTime);
                    if(Vector3.Distance(mouseObject.position,previousMousePositon) >= maxDistanceForSpawn){
                        previousMousePositon = mouseObject.position;
                        SpawnWall();
                        ReduceInk(1);
                        RotateWallTowardsNewWall();
                        rig.weight = 1f;
                    }
                    
                }
                if(playerInputController.GetTouchEnded()){
                    mouseObject.position = playerInputController.GetMousePoint();
                    mouseObject.gameObject.SetActive(false);
                    RotateWallTowardsNewWall();
                    rig.weight = 0f;
                }
            }
        }
        

        private void SpawnWall(){
            GameObject w = objectPoolingManager.SpawnFromPool(PoolObjectTag.Wall,playerInputController.GetMousePoint(),Quaternion.identity);
            Wall wall = w.GetComponent<Wall>();
            if(wall != null){
                if(!wallList.Contains(wall)){
                    wallList.Add(wall);
                }
            }
            
        }
        public void ReduceInk(int _value){
            currentInkAmount -= _value;
            if(currentInkAmount <= 0){
                currentInkAmount = 0;
            }
            onInkChange?.Invoke();
        }
        public void FillInk(int _value){
            if(currentInkAmount <= 0){
                currentInkAmount += _value;
                onInkChange?.Invoke();
            }
        }
        public float GetInkNormalizedValue(){
            return currentInkAmount/(float)maxInkAmount;
        }
        
        private void RotateWallTowardsNewWall(){
            for (int i = 0; i < wallList.Count; i++){
                if(i != 0){
                    if(i < wallList.Count - 1){
                        wallList[i].SetNextWall(wallList[i + 1].transform);
                        wallList.Remove(wallList[i]);
                    }else{
                        wallList[i].SetNextWall(mouseObject);
                    }
                }else{
                    wallList[i].SetNextWall(mouseObject);
                    wallList.Remove(wallList[0]);
                }
                
            }
            
        }
        


        
        
    }

}