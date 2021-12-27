using System;
using UnityEngine;

namespace SheildMaster {
    [CreateAssetMenu(fileName = "New Daily Reward",menuName = "ScriptableObject/Daily Reward")]
    public class DailyRewardSO : ScriptableObject {

        public int dayNumber;
        [TextArea(10,10)]
        public string discription;
        public DailyData dailyBonyData;

        public void SetDayNumber(int dayNum){
            dayNumber = dayNum;
        }
        
    }
    [Serializable]
    public class DailyData{
        public int coinAmount;
        public int shieldUseCount;
        public int killOneEnemyUseCount;
        public int dimondAmount;
    }

}