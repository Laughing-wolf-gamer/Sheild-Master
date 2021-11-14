using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace InkShield{
    [CreateAssetMenu(fileName = "New Level",menuName = "ScriptableObject/Level Data")]
    public class LevelDataSO : ScriptableObject {
        
        public LevelData levelData;
    }

}