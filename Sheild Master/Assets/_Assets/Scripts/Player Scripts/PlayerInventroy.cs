using UnityEngine;


namespace SheildMaster {
    public class PlayerInventroy : MonoBehaviour {

        [SerializeField] private Material playerMat;

        [SerializeField] private PlayerDataSO playerDataSO;

        private void Start(){
            SetcurrentSkin();
        }
        private void SetcurrentSkin()        {
            playerMat = playerDataSO.currentSkinMaterial;
        }
        
    }
}
