using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace SheildMaster {
    public class LevelManager : MonoBehaviour {
        [SerializeField] private PlayerDataSO playerDataSO;
        [SerializeField] private PlayerController player;
        [SerializeField] private List<LevelDataSO> levelDataList;
        // [SerializeField] private LevelData spawnLevelData;
        [SerializeField] private CinematicCamera cinematicCamera;
        [SerializeField] private Camera UiCamera;
        [SerializeField] private bool isTouchedtheScreen;
        [SerializeField] private AbilitySO sheildAbility,KillAbility;
        [SerializeField] private float flickerSpeed = 0.2f;
        private List<EnemyController> enemyList;
        private GameHandler gameHandler;
        private float surviveTime;
        private int randLevel;
        private int currenCoinCount;
        private LevelDataSO currentUsedLevelDataSO;
        private LevelData currentLevel;
        private UIHandler uIHandler;
        private int multiplier = 2;
        

        

        #region Singelton.......

        public static LevelManager current;
        private void Awake(){
            if(current == null){
                current = this;
            }else{
                Destroy(current.gameObject);
            }
            gameHandler = GetComponent<GameHandler>();

        }
        #endregion
        

        private void Start(){
            uIHandler = UIHandler.current;
            
            enemyList = new List<EnemyController>();
            randLevel = UnityEngine.Random.Range(0,levelDataList.Count);
            
            // switch(playerDataSO.GetLevelNumber()){
            //     case 1:
            //     break;
            //     case 25:
            //         SpawnTutorialLevels(2);
            //     break;
            //     case 50:
            //         SpawnTutorialLevels(2);
            //     break;
                
            // }
            // LevelStart();
            
        }
        public void LevelStart(){
            if(playerDataSO.GetLevelNumber() == 1){
                SpawnTutorialLevels(0);

            }
            else if(playerDataSO.GetLevelNumber() == 25 || playerDataSO.GetLevelNumber() == 50){
                SpawnTutorialLevels(2);
            }
            else{
                SpawnLevel();
            }
        }
        public void CheckForAllEnemyDead(){
            surviveTime ++;
            if(player.GetIsDead()){
                levelDataList[randLevel].playerDeathCountOnLevel++;
                gameHandler.SetGameOver(false);
                SetLostLevel();
                return;
            }
            for (int i = 0; i < enemyList.Count; i++){
                if(!enemyList[i].GetIsDead()){
                    return;
                }
            }
            if(!player.GetIsDead()){
                gameHandler.SetGameOver(true);
                currenCoinCount = enemyList.Count * 10;
                uIHandler.SetCurrentLevelEarnedCoins(currenCoinCount);    
                SetWinLevel();
                if(playerDataSO.GetLevelNumber() >= 25){
                    if((playerDataSO.GetLevelNumber() % 5) == 0){
                        sheildAbility.IncreaseAbility(2);
                    }
                }
                if(playerDataSO.GetLevelNumber() >= 60){
                    if((playerDataSO.GetLevelNumber() % 5) == 0){
                        KillAbility.IncreaseAbility(2);
                    }
                }
            }
        }

        
        public void StartFlicker(){
            StartCoroutine(RewardAmountChangeRoutine());
        }
        private IEnumerator RewardAmountChangeRoutine(){

            yield return new WaitForSeconds(flickerSpeed);
            
            if(multiplier == 2){
                multiplier = 5;
            }else if(multiplier == 5){
                multiplier = 10;
            }
            else if(multiplier >= 10f){
                multiplier = 2;
            }
            uIHandler.SetRewardAmountText(multiplier);
            yield return StartCoroutine(RewardAmountChangeRoutine());

        }
        public void CollectCoin(){
            AdManager.instance.DestroyBanner();
            gameHandler.AddCoin(currenCoinCount);
            uIHandler.UpdateCoinAmountUI();
        }
        public void AddTwiceMoney(){
            // Add Extra money after Ads..
            currenCoinCount *= multiplier;
            UIHandler.current.SetCurrentLevelEarnedCoins(currenCoinCount);
            UIHandler.current.UpdateCoinAmountUI();
            // StopCoroutine(nameof(RewardAmountChangeRoutine));
        }
        
        public void SetLevelEndResult(){
            if(player.GetIsDead()){
                AnayltyicsManager.current.OnGameLost_UnityAnayltics(levelDataList[randLevel].name,surviveTime,levelDataList[randLevel].playerDeathCountOnLevel);
            }else{
                AnayltyicsManager.current.OnGameWon_UnityAnayltics(levelDataList[randLevel].name,surviveTime);
            }
        }
        
        private void SpawnLevel(){
            if(playerDataSO.GetLostLevelIndex() >= 0){
                Debug.Log("Spawning the same Level");
                SpawnSameLevel();
            }else{
                Debug.Log("Spawning Random Level");
                SpawnRandomLevel();
            }
            
            enemyList = currentLevel.GetEnemieList();
            currenCoinCount = enemyList.Count;
            
        }
        private void SpawnSameLevel(){
            currentUsedLevelDataSO = levelDataList[playerDataSO.GetLostLevelIndex()];
            // currentUsedLevelDataSO = spawnLevelDataSO;
            currentLevel =  Instantiate(currentUsedLevelDataSO.levelData,transform.position,Quaternion.identity);
            // currentLevel = spawnLevelData;
            currentLevel.SpawnEnemyes(playerDataSO.GetSpawnPointList(),playerDataSO.GetSpawnAmount());
            SetLevelData();
            
        }
        private void SpawnTutorialLevels(int indexNumber){
            currentUsedLevelDataSO = levelDataList[indexNumber];
            // currentUsedLevelDataSO = spawnLevelDataSO;
            currentLevel = Instantiate(currentUsedLevelDataSO.levelData,transform.position,Quaternion.identity);
            // currentLevel = spawnLevelData;
            if(indexNumber == 2){
                currentLevel.SpawnEnemyes(indexNumber);
            }else{
                currentLevel.SpawnEnemyes();
            }

            enemyList = currentLevel.GetEnemieList();
            currenCoinCount = enemyList.Count;
            SetLevelData();
            // playerDataSO.RemakeNewSpawnPoint();
        }
        private void SpawnRandomLevel(){
            currentUsedLevelDataSO = levelDataList[randLevel];
            // currentUsedLevelDataSO = spawnLevelDataSO;
            currentLevel = Instantiate(currentUsedLevelDataSO.levelData,transform.position,Quaternion.identity);
            // currentLevel = spawnLevelData;
            currentLevel.SpawnEnemyes();
            SetLevelData();
        }
        
        public void StartGame(){
            isTouchedtheScreen = true;
            for (int e = 0; e < enemyList.Count; e++){
                enemyList[e].StartEnemy();
            }
        }
        
        public void SetGameViewcamera(Cinemachine.CinemachineVirtualCamera camer){
            cinematicCamera.SetGameCamera(camer);
        }
        
        
        public void EndGame(){
            for (int e = 0; e < enemyList.Count; e++){
                enemyList[e].EndGame();
            }
        }
        
        public void CheckifCanOpenAbiltiyPanal(){
            if(playerDataSO.GetLevelNumber() < 25){
                uIHandler.HideSheildAbilityIcon(true);
            }else{
                uIHandler.HideSheildAbilityIcon(false);
            }
            if(playerDataSO.GetLevelNumber() < 50){
                uIHandler.HideKillAbilityIcon(true);
            }else{
                uIHandler.HideKillAbilityIcon(false);
            }
            
        }
        
        public void KillOneEnemyBeforePlaying(){
            
            int rand = UnityEngine.Random.Range(0,enemyList.Count);
            if(enemyList[rand].GetIsDead()){
                rand = UnityEngine.Random.Range(0,enemyList.Count);
            }
            enemyList[rand].TakeHit(20);
            StartGame();
        }
        public void ArmourForPlayer(){
            StartGame();
            player.ActivateForceField();
        }
        private void SetLostLevel(){
            if(gameHandler.GetIslost()){
                SetLevelData();
            }
        }
        private void SetLevelData(){
            playerDataSO.SetLostLevelIndex(levelDataList.IndexOf(currentUsedLevelDataSO));
            // playerDataSO.SetLostLevelIndex(sce)
            playerDataSO.SetSpawnAmount(currentLevel.previousSpawnCount());
            playerDataSO.SetLostEnemySpawnPointsData(currentLevel.GetSpawnPoint());
        }
        private void SetWinLevel(){
            if(gameHandler.GetIsWon()){
                playerDataSO.SetLostLevelIndex(-1);
                // playerDataSO.RemakeNewSpawnPoint();
                playerDataSO.SetSpawnAmount(-1);
            }
        }
        
        
    }

}