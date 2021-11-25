using UnityEngine;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;

namespace InkShield {
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class MultiTargetCameraController : MonoBehaviour {

        [Header("Zooming")]
        
        [SerializeField] private float zoomLiminter = 50f;
        [SerializeField] private float minZoom = 90f;

        [SerializeField] private float maxZoom = 10f;

        [Header("Camera Movemnets")]
        [SerializeField] private float smoothSpeedTime = 0.5f;
        [SerializeField] private Vector3 offset;
        [Header("List Of Target")]
        [SerializeField]private List<Transform> targetsList;
        private Vector3 velocity;
        private CinemachineVirtualCamera zoomCamera;
        private void Start(){
            zoomCamera = GetComponent<CinemachineVirtualCamera>();
        }




        private void LateUpdate(){
            if(targetsList.Count == 0){
                return;
            }
            Zoom();
            Move();
        }
        private void Zoom(){
            float newZoom = Mathf.Lerp(maxZoom,minZoom,GetGreatestDistance()/zoomLiminter);
            zoomCamera.m_Lens.FieldOfView = Mathf.Lerp(zoomCamera.m_Lens.FieldOfView,newZoom,Time.deltaTime);
        }
        private float GetGreatestDistance(){
            var bounds = new Bounds(targetsList[0].position,Vector3.zero);
            for (int i = 0; i < targetsList.Count; i++){
                bounds.Encapsulate(targetsList[i].position);
            }
            return bounds.size.z + bounds.size.x;
        }
        private void Move(){
            Vector3 centerPoint = GetCenterPoint();
            Vector3 newPostion = centerPoint + offset;
            transform.position = Vector3.SmoothDamp(transform.position,newPostion,ref velocity,smoothSpeedTime * Time.deltaTime);
        }
        private Vector3 GetCenterPoint(){
            if(targetsList.Count == 0){
                return targetsList[0].position;
            }
            var bounds = new Bounds(targetsList[0].position,Vector3.zero);
            for (int i = 0; i < targetsList.Count; i++){
                bounds.Encapsulate(targetsList[i].position);
            }
            return bounds.center;
        }
        public void SetTargetToList(Transform targets){
            if(!targetsList.Contains(targets)){
                targetsList.Add(targets);
            }
            
        }
        
        public void RemoveTarget(Transform targets){
            if(targetsList.Contains(targets)){
                targetsList.Remove(targets);
            }
        }

        
    }

}