using UnityEngine;


namespace SheildMaster {
    public class PlayerInventroy : MonoBehaviour {

        [SerializeField] private SkinnedMeshRenderer playerSkinMat;
        [SerializeField] private PlayerDataSO playerDataSO;

        private void Start(){
            SetcurrentSkin();
        }
        private void SetcurrentSkin(){
            playerSkinMat.material = playerDataSO.playerSkinMaterial;
            
        }
        
    }
}
