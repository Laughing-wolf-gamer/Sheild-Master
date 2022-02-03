using System.IO;
using UnityEngine;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace SheildMaster{
    public enum ShopItemType{
        SKIN,
        ITEMS,
    }
    [CreateAssetMenu(fileName = "New Item",menuName = "ScriptableObject/Shop/Item")]
    public class ShopItemSO : ScriptableObject {

        public ShopItemType shopItemType;
        public Material playerSkin;
        public ShopItemData itemData;

        public bool isUsingTemprary;
        public int GetItemCost(){
            return itemData.cost;
        }
        public void SelectItem(){
            if(itemData.isBought){
                if(!itemData.isSelected){
                    itemData.isSelected = true;
                }
            }
        }
        public void SetUsingTemproray(bool value){
            isUsingTemprary = value;
        }
        public bool GetIsItemSelected(){
            return itemData.isSelected;
        }
        public bool GetIsItemBought(){
            return itemData.isBought;
        }
        public void UnSelectItem(){
            if(itemData.isSelected){
                itemData.isSelected = false;
                Save();
            }
        }
        public bool TryBuyitems(int coinValue){
            if(coinValue >= itemData.cost){
                itemData.isBought = true;
                return true;
            }
            return false;
        }

        [ContextMenu("Save")]
        public void Save(){
            
            string data = JsonUtility.ToJson(itemData,true);
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Create(string.Concat(Application.persistentDataPath,"/",name));
            formatter.Serialize(file,data);
            file.Close();
        }

        [ContextMenu("Load")]
        public void Load(){
            
            if(File.Exists((string.Concat(Application.persistentDataPath,"/",name)))){
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream Stream = File.Open(string.Concat(Application.persistentDataPath,"/",name),FileMode.Open);
                JsonUtility.FromJsonOverwrite(formatter.Deserialize(Stream).ToString(),itemData);
                Stream.Close();
            }
        }
        
    }
    [System.Serializable]
    public class ShopItemData{
        public int cost;
        public bool isSelected;
        public bool isBought;
    }
}