using UnityEngine;
using GamerWolf.Utils;
using UnityEngine.EventSystems;
namespace SheildMaster {
    public class PlayerInputController : MonoBehaviour {
        
        [SerializeField] private LayerMask hitMask;
        [SerializeField] private GameObject[] playText;
        [SerializeField] private LevelManager levelManager;
        [SerializeField] private TutorialManager tutorialManager;
        private bool isTouchDown;
        private bool isTouchMoving;
        private bool isTouchEnded;

        private Camera m_Camera;
        private Vector3 MousePoint;
        
        private bool isFirstTouchUp;
        
        
        public static PlayerInputController current;

        private void Awake(){
            if(current == null){
                current = this;
            }
            m_Camera = Camera.main;
            isFirstTouchUp = false;
        }
        public void GetPcInput(){
            if(Input.GetMouseButtonDown(0)){
                for (int i = 0; i < playText.Length; i++){
                    if(playText[i].activeInHierarchy){
                        playText[i].SetActive(false);
                    }
                }
                tutorialManager.ShowInitTutorailWindow(false);
            }
            if(!EventSystem.current.IsPointerOverGameObject()){
                isTouchDown = Input.GetMouseButtonDown(0) && GetMousePoint() != Vector3.zero;
                isTouchMoving = Input.GetMouseButton(0) && GetMousePoint() != Vector3.zero;
                isTouchEnded = Input.GetMouseButtonUp(0) && GetMousePoint() != Vector3.zero;
                
                if(isTouchEnded){
                    if(!isFirstTouchUp){
                        isFirstTouchUp = true;
                        if(levelManager != null){
                            levelManager.StartGame();
                        }
                    }
                }
            }
            
            
        }
        
        public void GetMobileInputs(){
            if(Input.GetMouseButtonDown(0)){
                for (int i = 0; i < playText.Length; i++){
                    if(playText[i].activeInHierarchy){
                        playText[i].SetActive(false);
                    }
                }
                tutorialManager.ShowInitTutorailWindow(false);
            }
            if(Input.touchCount > 0){
                Touch touch = Input.touches[0];
                int id = touch.fingerId;
                if(!EventSystem.current.IsPointerOverGameObject(id)){
                    isTouchDown = (touch.phase == TouchPhase.Began && GetMousePoint() != Vector3.zero) ? true : false;
                    isTouchMoving = (touch.phase == TouchPhase.Moved && GetMousePoint() != Vector3.zero) ? true : false;
                    isTouchEnded = (touch.phase == TouchPhase.Ended && GetMousePoint() != Vector3.zero) ? true : false;
                    
                    if(isTouchEnded){
                        if(!isFirstTouchUp){
                            isFirstTouchUp = true;
                            if(levelManager != null){
                                levelManager.StartGame();
                            }
                        }
                    }
                }
            }
        }
        
        public Vector3 GetMousePoint(){
            #if UNITY_EDITOR
            Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);
            #else
            Touch touch = Input.GetTouch(0);
            Ray ray = m_Camera.ScreenPointToRay(touch.position);
            #endif
            Vector3 point = Vector3.zero;
            if(Physics.Raycast(ray,out RaycastHit hit,300f,hitMask,QueryTriggerInteraction.Ignore)){
                Debug.DrawLine(ray.origin,point,Color.cyan);
                point = hit.point;
            }else{
                point = Vector3.zero;
            }
            
            return point;

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
