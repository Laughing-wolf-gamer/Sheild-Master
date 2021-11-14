using UnityEngine;
using Cinemachine;



namespace InkShield {
    public class CinematicCamera : MonoBehaviour {
        

        [SerializeField] private CinemachineVirtualCamera gameViewCamera,playerViewCamera;


        public void OnGameEnd(){
            gameViewCamera.Priority = 5;
            playerViewCamera.Priority = 10;
        }
        public void OnGameStart(){
            gameViewCamera.Priority = 10;
            gameViewCamera.Priority = 5;

        }
        
    }

}