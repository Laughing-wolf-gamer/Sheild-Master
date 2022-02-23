using TMPro;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using System.Collections;

namespace GamerWolf.Utils {
    public class TutorialManager : MonoBehaviour {
        [SerializeField] private float dealy = 0.6f;
        [SerializeField] private GameObject tutorailWindow;

        [Header("Character Info")]
        [SerializeField] private DOTweenAnimation characterTutorialWindow;
        [SerializeField] private TextMeshProUGUI characterNameText,infoText;
        private void Start(){
            characterTutorialWindow.autoPlay = false;
        }
        public void ShowCharacterTutorial(bool _showTutorailWindow,string _nameText = null,string _infoText = null){
            characterTutorialWindow.autoPlay = false;
            characterTutorialWindow.gameObject.SetActive(false);
            StartCoroutine(ShowTutorialRoutine(_showTutorailWindow,_nameText,_infoText));
        }
        private IEnumerator ShowTutorialRoutine(bool _showTutorailWindow,string _nameText = null,string _infoText = null){
            yield return new WaitForSeconds(dealy);
            characterTutorialWindow.gameObject.SetActive(_showTutorailWindow);
            characterTutorialWindow.DOPlay();
            if(_showTutorailWindow){
                Time.timeScale = 0f;
                Debug.Log("Tutorial for super Enemy");
                characterNameText.SetText(_nameText);
                infoText.SetText(_infoText);
            }else{
                characterNameText.SetText(string.Empty);
                infoText.SetText(string.Empty);
            }
        }
        public void ShowInitTutorailWindow(bool show){
            tutorailWindow.SetActive(show);
        }
        public void Resume(){
            Time.timeScale = 1f;
        }
        
    }
}
