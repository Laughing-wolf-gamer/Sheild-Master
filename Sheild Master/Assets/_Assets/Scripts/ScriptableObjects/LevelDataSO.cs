using UnityEngine;

namespace SheildMaster{
    
    [CreateAssetMenu(fileName = "New Level",menuName = "ScriptableObject/Level Data")]
    public class LevelDataSO : ScriptableObject {

        public int playerDeathCountOnLevel;
        public LevelData levelData;
        
    }

}