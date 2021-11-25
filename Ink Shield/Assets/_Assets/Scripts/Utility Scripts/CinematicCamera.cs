using UnityEngine;
using Cinemachine;



namespace InkShield {
    public class CinematicCamera : MonoBehaviour {
        

        [SerializeField] private CinemachineVirtualCamera playerViewCamera;
        private CinemachineVirtualCamera gameViewCamera;

        public static CinematicCamera current;
        private void Awake(){
            if(current == null){
                current = this;
            }
        }
        public void OnGameEnd(){
            gameViewCamera.Priority = 5;
            playerViewCamera.Priority = 10;
        }
        public void OnGameStart(){
            gameViewCamera.Priority = 10;
            gameViewCamera.Priority = 5;

        }
        public void SetGameCamera(CinemachineVirtualCamera camera){
            gameViewCamera = camera;
        }

        
    }

}