using System.IO;
using UnityEngine;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
namespace SheildMaster {
    public enum AbilityType{
        Kill_one_Enemy,
        One_Bullet_Sheild,
    }
    
    [CreateAssetMenu(fileName = "New Ability",menuName = "ScriptableObject/Ability")]
    public class AbilitySO : ScriptableObject {
        public AbilityType abilityType;

        [TextArea(10,10)]
        public string discription;
        public AbilityCostReciepe cost;
        public Sprite abilitySpriteDisplay;
        
        [SerializeField] private AbilitySaveData abilitySaveData;
        public void IncreaseAbility(int abilityCount){
            abilitySaveData.abilityUseCount += abilityCount;
            if(abilityCount > 0){
                if(!abilitySaveData.isUnlocked){
                    abilitySaveData.isUnlocked = true;
                }
            }
        }
        public void UseAbility(){
            if(CanUseAbility()){
                abilitySaveData.abilityUseCount--;
            }
            if(abilitySaveData.abilityUseCount <= 0){
                abilitySaveData.abilityUseCount = 0;
                if(abilitySaveData.isUnlocked){
                    abilitySaveData.isUnlocked = false;
                }
            }
            
        }
        public bool CanUseAbility(){
            if(abilitySaveData.isUnlocked){
                if(abilitySaveData.abilityUseCount > 0){
                    return true;
                }
            }
            return false;

        }
        public bool CanBuyItem(int coinAmount,int dimondAmount,int useCount){
            if(coinAmount >= cost.coinCount && dimondAmount >= cost.dimondCount){
                if(!abilitySaveData.isUnlocked){
                    abilitySaveData.isUnlocked = true;
                }
                IncreaseAbility(useCount);
                return true;
            }
            return false;
        }
        public int GetAbilityUseCount(){
            return abilitySaveData.abilityUseCount;
        }
        [ContextMenu("Save")]
        public void Save(){
            string data = JsonUtility.ToJson(abilitySaveData,true);
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Create(string.Concat(Application.persistentDataPath,"/","Player Abiliy",name));
            formatter.Serialize(file,data);
            file.Close();
        }

        [ContextMenu("Load")]
        public void Load(){
            if(File.Exists((string.Concat(Application.persistentDataPath,"/","Player Abiliy",name)))){
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream Stream = File.Open(string.Concat(Application.persistentDataPath,"/","Player Abiliy",name),FileMode.Open);
                JsonUtility.FromJsonOverwrite(formatter.Deserialize(Stream).ToString(),abilitySaveData);
                Stream.Close();
            }
        }
        [ContextMenu("Clear Ability Data")]
        public void ClearAbility(){
            UseAbility();
            Save();
        }
    }
    [System.Serializable]
    public class AbilitySaveData{
        public bool isUnlocked = false;

        public int abilityUseCount = 10;
    }
    [System.Serializable]
    public class AbilityCostReciepe{
        public int coinCount = 20000;
        public int dimondCount = 20;
    }

}