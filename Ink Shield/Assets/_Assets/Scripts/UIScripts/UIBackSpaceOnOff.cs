using UnityEngine;


namespace GamerWolf.Utils {
    public class UIBackSpaceOnOff : MonoBehaviour {
        
        [SerializeField] private GameObject windowToActive,windowToDeactive;
        
        private void Update(){
            if(Input.GetKeyDown(KeyCode.Escape)){
                windowToActive.SetActive(true);
                windowToDeactive.SetActive(false);
            }
        }
        
    }
}
