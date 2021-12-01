using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SheildMaster;
public class Testing : MonoBehaviour {
    [SerializeField] private float maxMoveDist;
    [SerializeField] private Transform mouseObject;
    [SerializeField] private ExpandingWall expandingWall;
    [SerializeField] private PlayerInputController playerInput;
    [SerializeField] private Vector3 previousDir;
    [SerializeField] private Vector3 initPoint;
    private ExpandingWall currentWall;
    private void Update(){
        playerInput.GetPcInput();
        mouseObject.position = playerInput.GetMousePoint();
        if(playerInput.GetTouchStarted()){
            initPoint = mouseObject.position;
            currentWall = Instantiate(expandingWall,mouseObject.position,Quaternion.identity);
            previousDir = (initPoint - mouseObject.position).normalized;
        }
        if(playerInput.GetTouchMoving()){
            Vector3 currentDir = (previousDir - mouseObject.position).normalized;
            currentWall.SetExpandDir(mouseObject.position);
            if(currentDir != previousDir){
                previousDir = currentDir;
                if(Vector3.Distance(initPoint,mouseObject.position) >= maxMoveDist){
                    ExpandingWall wall = Instantiate(expandingWall,currentWall.GetNewWallSpawnPoint(),Quaternion.identity);
                    if(wall != currentWall){
                        currentWall = wall;
                    }
                    initPoint = mouseObject.position;
                }
            }
            
        }
    }
    
    
    
}
