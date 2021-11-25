using System.IO;
using UnityEngine;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
namespace InkShield {
    public enum AbilityType{
        Kill_one_Enemy,
        One_Bullet_Armour,
    }
    
    [CreateAssetMenu(fileName = "New Ability",menuName = "ScriptableObject/Ability")]
    public class AbilitySO : ScriptableObject {

        public AbilityType abilityType;

        [TextArea(10,10)]
        public string discription;
        public float increaseSpeed;
        public bool isUnlocked = false;

        public float maxUnlockeAmount = 50;
        public float currentValue = 0f;
        public void IncreaseAbiliyValue(float value){
            currentValue += value;
            if(currentValue >= maxUnlockeAmount){
                currentValue = maxUnlockeAmount;
                if(!isUnlocked){
                    isUnlocked = true;
                }
            }
        }
        public void UseAbility(){
            currentValue = maxUnlockeAmount;
            if(currentValue <= 0f){
                currentValue = 0f;
                isUnlocked = false;
            }
        }
        public float GetAbilityValueNormalized(){
            return currentValue/maxUnlockeAmount;
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