using UnityEngine;
using UnityEngine.EventSystems;

namespace InkShield {
    public class PlayerInputController : MonoBehaviour {
        
        [SerializeField] private LayerMask hitMask;
        
        private bool isTouchDown;
        private bool isTouchMoving;
        private bool isTouchEnded;

        private Camera m_Camera;
        private Vector3 MousePoint;
        
        private bool isFirstTouch;


        public static PlayerInputController current;


        private void Awake(){
            if(current == null){
                current = this;
            }
            m_Camera = Camera.main;
            isFirstTouch = false;
        }

        
        
        public void GetPcInput(){

            if(!EventSystem.current.IsPointerOverGameObject()){
                isTouchDown = Input.GetMouseButtonDown(0);
                isTouchMoving = Input.GetMouseButton(0);
                isTouchEnded = Input.GetMouseButtonUp(0);
                if(isTouchEnded){
                    if(!isFirstTouch){
                        isFirstTouch = true;
                        LevelManager.current.SubscribeToOnFirstTouch();
                    }
                }
            }
            
            
        }
        public void GetMobileInput(){
            if(Input.touchCount > 0){
                Touch touch = Input.touches[0];
                int id = touch.fingerId;
                if(!EventSystem.current.IsPointerOverGameObject(id)){
                    isTouchDown = touch.phase == TouchPhase.Began ? true : false;
                    isTouchMoving = touch.phase == TouchPhase.Moved ? true : false;
                    isTouchEnded = touch.phase == TouchPhase.Ended ? true : false;
                    if(isTouchEnded){
                        if(!isFirstTouch){
                            isFirstTouch = true;
                            LevelManager.current.SubscribeToOnFirstTouch();
                        }
                    }
                }
            }
            

        }
        public Vector3 GetMousePoint(){
            #if UNITY_EDITOR
            Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);
            #else
            Touch touch = Input.touches[0];
            Ray ray = m_Camera.ScreenPointToRay(touch.position);
            #endif
            // Plane groundPlane = new Plane(Vector3.up,Vector3.zero);
            if(Physics.Raycast(ray,out RaycastHit hit,float.MaxValue,hitMask)){
                Vector3 point = hit.point;
                Debug.DrawLine(ray.origin,point);
                return point;
            }else{
                return ray.GetPoint(20);
            }

        }


        public bool GetTouchStarted(){
            return isTouchDown;
        }
        public bool GetTouchMoving(){
            return isTouchMoving;
        }
        public bool GetTouchEnded(){
            return isTouchEnded;
        }
        
        
    }

}
