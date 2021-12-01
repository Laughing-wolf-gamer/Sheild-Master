using UnityEngine;


namespace SheildMaster {
    public class PlayerInventroy : MonoBehaviour {

        [SerializeField] private SkinnedMeshRenderer playerSkinMat,playerClothMat,playerBeltMat;

        [SerializeField] private PlayerDataSO playerDataSO;

        private void Start(){
            SetcurrentSkin();
        }
        private void SetcurrentSkin()        {
            playerSkinMat.material = playerDataSO.playerSkinMaterial;
            playerClothMat.material = playerDataSO.playerClothMaterial;
            playerBeltMat.material = playerDataSO.playerBeltMat;
        }
        
    }
}
