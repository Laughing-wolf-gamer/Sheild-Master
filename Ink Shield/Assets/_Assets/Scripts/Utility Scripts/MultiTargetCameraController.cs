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
        [SerializeField] private List<Transform> targetsList;
        [SerializeField] private Vector3 offset;
        private Vector3 velocity;
        private CinemachineVirtualCamera zoomCamera;
        private bool isFollow;

        #region Singelton.......
        public static MultiTargetCameraController current;
        private void Awake(){
            if(current == null){
                current = this;
            }else{
                Destroy(current.gameObject);
            }
        }

        #endregion
        private void Start(){
            zoomCamera = GetComponent<CinemachineVirtualCamera>();
            targetsList = new List<Transform>();
        }




        private void LateUpdate(){
            if(targetsList.Count == 0){
                return;
            }
            Zoom();
            Move();
        }
        private void Zoom(){
            Debug.Log(GetGreatestDistance());
            float newZoom = Mathf.Lerp(maxZoom,minZoom,GetGreatestDistance());
            zoomCamera.m_Lens.FieldOfView = Mathf.Lerp(zoomCamera.m_Lens.FieldOfView,newZoom,Time.deltaTime);
        }
        private float GetGreatestDistance(){
            var bounds = new Bounds(targetsList[0].position,Vector3.zero);
            for (int i = 0; i < targetsList.Count; i++){
                bounds.Encapsulate(targetsList[i].position);
            }
            Debug.Log(bounds.size);
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
            isFollow = true;
        }
        public void GameOver(){
            isFollow = false;
        }

        
    }

}