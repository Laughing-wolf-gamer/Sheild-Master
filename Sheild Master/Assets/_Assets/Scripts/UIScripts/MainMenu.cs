using UnityEngine;
using UnityEngine.Events;


namespace SheildMaster {
    public class MainMenu : MonoBehaviour {
        [SerializeField] private UnityEvent onGameStart;

        private void Start(){
            onGameStart?.Invoke();
        }
        public void PlayGame(){
            LevelLoader.current.PlayLevel(SceneIndex.Game_Scene);
        }
        private void Update(){
            if(Input.GetKeyDown(KeyCode.Escape)){
                Application.Quit();
            }
        }
        
        
    }

}