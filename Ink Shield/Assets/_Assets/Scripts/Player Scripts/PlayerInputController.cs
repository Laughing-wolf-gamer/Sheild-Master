using UnityEngine;
using UnityEngine.EventSystems;

namespace InkShield {
    public class PlayerInputController : MonoBehaviour {
        

        private bool isTouchDown;
        private bool isTouchMoving;
        private bool isTouchUp;

        private Camera m_Camera;
        private Vector3 MousePoint;
        private Touch touch;
        private bool isFirstTouch;
        private void Awake(){
            m_Camera = Camera.main;
            isFirstTouch = false;
        }

        private void Update(){
        #if UNITY_EDITOR
            Pc();
        #else
            Mobile();
        #endif

        }
        
        private void Pc(){

            if(!EventSystem.current.IsPointerOverGameObject()){
                isTouchDown = Input.GetMouseButtonDown(0);
                isTouchMoving = Input.GetMouseButton(0);
                isTouchUp = Input.GetMouseButtonUp(0);
            }
            if(isTouchUp){
                if(!isFirstTouch){
                    isFirstTouch = true;
                    LevelManager.current.SubscribeToOnFirstTouch();
                }
            }
            
        }
        private void Mobile(){
            if(Input.touchCount > 0){
                touch = Input.GetTouch(0);
                int firstTouchId = touch.fingerId;
                if(!EventSystem.current.IsPointerOverGameObject(firstTouchId)){
                    if(!isFirstTouch){
                        isFirstTouch = true;
                        LevelManager.current.SubscribeToOnFirstTouch();
                    }
                    if(touch.phase == TouchPhase.Began){
                        isTouchDown = true;
                        isTouchMoving = false;
                        isTouchUp = false;
                    }else{
                        isTouchDown = false;
                    }


                    if(touch.phase == TouchPhase.Moved){
                        isTouchMoving = true;
                        isTouchDown = false;
                        isTouchUp = false;
                    }else{
                        isTouchMoving = false;
                    }

                    if(touch.phase == TouchPhase.Ended){
                        isTouchUp = true;
                        isTouchDown = false;
                        isTouchMoving = false;
                        if(!isFirstTouch){
                            isFirstTouch = true;
                            LevelManager.current.SubscribeToOnFirstTouch();
                        }
                    }else{
                        isTouchUp = false;
                    }
                }
            }
            

        }
        public Vector3 GetMousePoint(){
            #if UNITY_EDITOR
            Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);
            #else
            Ray ray = m_Camera.ScreenPointToRay(touch.GetTouch(0));
            #endif
            Plane groundPlane = new Plane(Vector3.up,Vector3.zero);
            float maxDist;
            if(groundPlane.Raycast(ray,out maxDist)){
                Vector3 point = ray.GetPoint(maxDist);
                Debug.DrawLine(ray.origin,point);
                return point;
            }else{
                return Vector3.zero;
            }

        }


        public bool GetTouchStarted(){
            return isTouchDown;
        }
        public bool GetTouchMoving(){
            return isTouchMoving;
        }
        public bool GetTouchEnded(){
            return isTouchUp;
        }
        
        
    }

}
