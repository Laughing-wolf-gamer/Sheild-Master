using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
namespace SheildMaster{
    
    [CreateAssetMenu(fileName = "New Level",menuName = "ScriptableObject/Level Data")]
    public class LevelDataSO : ScriptableObject {

        public int playerDeathCountOnLevel;
        public LevelData levelData;
        // [SerializeField] private LostData lostData;
        // public void SetLostBool(bool value){
        //     this.lostData.lostOnThisLevel = value;
            
        // }
        // public void SetSpawnAmount(int amount){
        //     this.lostData.spawnAmount = amount;
        // }
        // public void SetLostData(List<Vector3> spawnPointList) {
        //     this.lostData.spawnPointlist = spawnPointList;
        // }
        // public int GetSpawnAmount(){
        //     return lostData.spawnAmount;
        // }
        // public bool GetIsLostOnLevel(){
        //     return lostData.lostOnThisLevel;
        // }
        // public List<Vector3> GetSpawnPointList(){
        //     return lostData.spawnPointlist;
        // }
        // public void RemakeNewSpawnPoint(){
        //     lostData.spawnPointlist = new List<Vector3>();
        //     SetSpawnAmount(0);
        //     SetLostBool(false);
        // }
        // [ContextMenu("Save")]
        // public void Save(){
        //     string data = JsonUtility.ToJson(lostData,true);
        //     BinaryFormatter formatter = new BinaryFormatter();
        //     FileStream file = File.Create(string.Concat(Application.persistentDataPath,"/","Level Data"));
        //     formatter.Serialize(file,data);
        //     file.Close();
        // }

        // [ContextMenu("Load")]
        // public void Load(){
        //     if(File.Exists((string.Concat(Application.persistentDataPath,"/","Level Data")))){
        //         BinaryFormatter formatter = new BinaryFormatter();
        //         FileStream Stream = File.Open(string.Concat(Application.persistentDataPath,"/","Level Data"),FileMode.Open);
        //         JsonUtility.FromJsonOverwrite(formatter.Deserialize(Stream).ToString(),lostData);
        //         Stream.Close();
        //     }
        // }
        
    }
    // [System.Serializable]
    // public class LostData{
    //     public int spawnAmount;
    //     public bool lostOnThisLevel;
    //     public List<Vector3> spawnPointlist;
    // }
    

}