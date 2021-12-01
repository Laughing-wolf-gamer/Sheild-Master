using UnityEngine;
using UnityEngine.Events;

namespace GamerWolf.Utils {
    public class UIBackSpaceOnOff : MonoBehaviour {
        
        [SerializeField] private UnityEvent onEscape;
        
        private void Update(){
            if(Input.GetKeyDown(KeyCode.Escape)){
                onEscape?.Invoke();
            }
        }
        
    }
}
