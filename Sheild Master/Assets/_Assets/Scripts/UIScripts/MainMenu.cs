using UnityEngine;
using UnityEngine.Events;


namespace SheildMaster {
    public class MainMenu : MonoBehaviour {
        [SerializeField] private UnityEvent onGameStart;
        [SerializeField] private GameObject quitWindow,shopWindow,dailyRewardWindow;
        private void Start(){
            onGameStart?.Invoke();
        }
        public void PlayGame(){
            LevelLoader.current.PlayLevel(SceneIndex.Game_Scene);
        }
        private void Update(){
            if(Input.GetKeyDown(KeyCode.Escape)){
                if(CanQuit()){
                    quitWindow.SetActive(true);
                }
            }
        }
        private bool CanQuit(){
            return !(shopWindow.activeInHierarchy || dailyRewardWindow.activeInHierarchy);
        }
        public void Quit(){
            Application.Quit();
        }
        
        
        
    }

}