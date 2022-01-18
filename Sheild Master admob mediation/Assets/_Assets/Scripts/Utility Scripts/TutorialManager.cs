using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace GamerWolf.Utils {
    public class TutorialManager : MonoBehaviour {

        [SerializeField] private GameObject tutorailWindow;

        [Header("Character Info")]
        [SerializeField] private GameObject characterTutorialWindow;
        [SerializeField] private TextMeshProUGUI characterNameText,infoText;
        
        public void ShowCharacterTutorial(bool _showTutorailWindow,string _nameText = null,string _infoText = null){
            characterTutorialWindow.SetActive(_showTutorailWindow);
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
