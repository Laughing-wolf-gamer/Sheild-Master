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
        // public float increaseSpeed;
        public bool isUnlocked = false;

        public int abilityUseCount = 10;
        // public float currentValue = 0f;
        public void IncreaseAbility(int abilityCount){
            abilityUseCount += abilityCount;
            if(abilityCount > 0){
                if(!isUnlocked){
                    isUnlocked = true;
                }
            }
        }
        public void UseAbility(){
            abilityUseCount--;
            if(abilityUseCount <= 0){
                abilityUseCount = 0;
                if(isUnlocked){
                    isUnlocked = false;
                }
            }
            
        }
        public int GetAbilityUseCount(){
            return abilityUseCount;
        }
        [ContextMenu("Save")]
        public void Save(){
            string data = JsonUtility.ToJson(this,true);
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Create(string.Concat(Application.persistentDataPath,"/","Player Abiliy",name));
            formatter.Serialize(file,data);
            file.Close();
        }

        [ContextMenu("Load")]
        public void Load(){
            if(File.Exists((string.Concat(Application.persistentDataPath,"/","Player Data")))){
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream Stream = File.Open(string.Concat(Application.persistentDataPath,"/","Player Abiliy",name),FileMode.Open);
                JsonUtility.FromJsonOverwrite(formatter.Deserialize(Stream).ToString(),this);
                Stream.Close();
            }
        }
        [ContextMenu("Clear Ability Data")]
        public void ClearAbility(){
            UseAbility();
            Save();
        }
    }

}