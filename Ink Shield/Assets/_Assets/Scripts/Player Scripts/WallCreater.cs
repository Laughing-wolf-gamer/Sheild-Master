using System;
using UnityEngine;
using GamerWolf.Utils;
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

        

        #endregion

        #region Private Variables.....
        private int currentInkAmount;
        private ObjectPoolingManager objectPoolingManager;
        
        private List<Wall> wallList;
        private PlayerInputController playerInputController;
        private Vector3 previousMousePositon;

        #endregion


        #region System Events.....
        public Action onInkUse;

        #endregion



        private void Awake(){
            playerInputController = GetComponent<PlayerInputController>();
        }
        private void Start(){
            wallList = new List<Wall>();
            currentInkAmount = maxInkAmount;
            objectPoolingManager = ObjectPoolingManager.current;
            onInkUse += ()=>{
                UIHandler.current.SetInkTankValue(GetInkNormalizedValue());
            };
        }
       
        public void TryDrawWall(){
            if(currentInkAmount > 0){
                if(playerInputController.GetTouchStarted()){
                    mouseObject.position = Vector3.MoveTowards(mouseObject.position,playerInputController.GetMousePoint(),mouseObjectMoveSpeed * Time.deltaTime);
                    previousMousePositon = mouseObject.position;
                    SpawnWall();
                    ReduceInkValue(1);
                }else if(playerInputController.GetTouchMoving()){
                    mouseObject.position = Vector3.MoveTowards(mouseObject.position,playerInputController.GetMousePoint(),mouseObjectMoveSpeed * Time.deltaTime);
                    if(Vector3.Distance(previousMousePositon,mouseObject.position) >= maxDistanceForSpawn){
                        SpawnWall();
                        previousMousePositon = mouseObject.position;
                        ReduceInkValue(1);
                        RotateWallTowardsNewWall();
                    }
                    
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
        public void ReduceInkValue(int _value){
            currentInkAmount -= _value;
            if(currentInkAmount <= 0){
                currentInkAmount = 0;
            }
            onInkUse?.Invoke();
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