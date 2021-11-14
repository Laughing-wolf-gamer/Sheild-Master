using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
namespace InkShield {

    public class GameHandler : MonoBehaviour {
        

        [Header("Events")]
        [SerializeField] private UnityEvent onGameStart;
        [SerializeField] private UnityEvent onGamePlaying,onGamePause,onGameResume,onGameEnd,onWin,onLoss;


        [Header("Testing Variables")]
        [SerializeField] private bool isGamePlaying;
        [SerializeField] private bool isGameOver;
        [SerializeField] private bool isWon;

        #region Singelton.........
        public static GameHandler current;
        
        private void Awake(){
            if(current == null){
                current = this;
            }else{
                Destroy(current.gameObject);
            }
        }

        #endregion

        private void Start(){
            StartCoroutine(nameof(GameStartRoutine));
        }


        private IEnumerator GameStartRoutine(){
            onGameStart?.Invoke();
            while(!isGamePlaying){
                yield return null;
            }
            

            StartCoroutine(GamePlayRoutine());
        }
        private IEnumerator GamePlayRoutine(){
            onGamePlaying?.Invoke();
            while(!isGameOver){
                yield return null;

            }
            onGameEnd?.Invoke();
            yield return new WaitForSeconds(1.5f);
            if(isWon){
                onWin?.Invoke();
            }else{
                onLoss?.Invoke();
            }
        }


        public void PlayGame(){
            isGamePlaying = true;
            isGameOver = false;
        }
        public void SetGameOver(bool isWon){
            isGamePlaying = false;
            isGameOver = true;
            this.isWon = isWon;

        }
        public void Restart(){
            if(isGameOver){
                SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
            }
        }
        
        
    }

}