using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace InkShield {
    public class MainMenu : MonoBehaviour {

        [SerializeField] private UnityEvent onGameStart;

        private void Start(){
            onGameStart?.Invoke();
        }
        public void PlayGame(){
            LevelLoader.current.PlayLevel(SceneIndex.Game_Scene);
        }
        
    }

}