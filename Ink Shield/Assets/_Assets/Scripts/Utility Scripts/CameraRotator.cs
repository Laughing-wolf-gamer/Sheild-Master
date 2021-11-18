using UnityEngine;


namespace InkShield {
    public class CameraRotator : MonoBehaviour {
        
        [SerializeField] private float rotationSpeed = 50f;
        [SerializeField] private bool activateRotations;

        private Vector3 origRotation;

        private float rotX = 0f;
        private float rotY = 0f;
        private Camera cam;
        private float dir = -1f;
        private Vector3 previousPos;
        private PlayerInputController playerInputController;
        private void Awake(){
            cam = Camera.main;
            playerInputController = PlayerInputController.current;
        }
        private void Start(){
            origRotation = transform.eulerAngles;
            rotX = origRotation.x;
            rotY = origRotation.y;
            activateRotations = false;
        }
        private void Update(){
            if(activateRotations){
                if(playerInputController.GetTouchStarted()){
                    previousPos = GetmousePos();
                }
                if(playerInputController.GetTouchMoving()){
                    float deltaX = previousPos.x - GetmousePos().x;
                    float deltaY = previousPos.y - GetmousePos().y;
                    rotX -= deltaY * Time.deltaTime * rotationSpeed * dir;
                    rotY += deltaX * Time.deltaTime * rotationSpeed * dir;
                    transform.eulerAngles = new Vector3(rotX,rotY,0f);
                }
            }
        }
        private Vector3 GetmousePos(){
            Vector3 poin = cam.ScreenToViewportPoint(Input.mousePosition);
            return new Vector3(poin.x,poin.y,0f);
        }
    }

}