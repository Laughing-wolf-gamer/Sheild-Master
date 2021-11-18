using UnityEngine;


namespace InkShield{
    [CreateAssetMenu(fileName = "New Player Data",menuName = "ScriptableObject/Player Data")]
    public class PlayerDataSO : ScriptableObject {
        
        public int maxCoinCount;
        public int currentExperience;

        public void AddCoins(int value){
            maxCoinCount += value;
        }
        public void ReduceCoins(int value){
            maxCoinCount -= value;
            if(maxCoinCount <= 0f){
                maxCoinCount = 0;
            }
        }
        public int GetCoinValue(){
            return maxCoinCount;
        }
        public int GetExperience(){
            return currentExperience;
        }
        
    }

}