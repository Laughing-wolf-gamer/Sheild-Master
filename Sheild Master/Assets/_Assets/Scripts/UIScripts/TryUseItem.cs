using UnityEngine;

namespace GamerWolf.Utils{
    public class TryUseItem : MonoBehaviour {

        [SerializeField] private TimeManager timeManager;




        public void TrySkin(){
            timeManager.Click();
        }
        
    }

}