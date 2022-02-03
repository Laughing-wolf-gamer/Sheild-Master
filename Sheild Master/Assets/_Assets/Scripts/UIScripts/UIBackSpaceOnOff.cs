using UnityEngine;
using UnityEngine.Events;

namespace GamerWolf.Utils {
    public class UIBackSpaceOnOff : MonoBehaviour {
        
        [SerializeField] private UnityEvent onEscape;
        
        private void Update(){
            if(Input.GetKeyDown(KeyCode.Escape)){
                SheildMaster.AudioManager.current.PlayOneShotMusic(SheildMaster.SoundType.ButtonClickSound);        
                InvokeEscape();
            }
        }
        public void InvokeEscape(){
            onEscape?.Invoke();
        }
        
    }
}
