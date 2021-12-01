using System;
using UnityEngine;

namespace SheildMaster {
    [CreateAssetMenu(fileName = "New Daily Reward",menuName = "ScriptableObject/Daily Reward")]
    public class DailyRewardSO : ScriptableObject {

        public DayOfWeek dayOfWeek;
        [TextArea(10,10)]
        public string discription;
        public DailyData dailyBonyData;

        
    }
    [Serializable]
    public class DailyData{
        public int coinAmount;
        public int shieldUseCount;
        public int killOneEnemyUseCount;
        public int dimondAmount;
    }

}