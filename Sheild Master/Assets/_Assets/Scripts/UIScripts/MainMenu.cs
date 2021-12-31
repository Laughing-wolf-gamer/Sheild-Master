using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
namespace SheildMaster {
    public class MainMenu : MonoBehaviour {
        [SerializeField] private GameObject quitMenu,shopMenu,settingsMenu;
        [SerializeField] private SkinnedMeshRenderer playDemoRenderer;
        [SerializeField] private TextMeshProUGUI coinAmount,dimondAmount;
        [SerializeField] private UnityEvent onGameStart;
        [SerializeField] private PlayerDataSO playerData;
        [Space(20)]
        [Header("Animations")]
        [SerializeField] private PopOutAnimationsItems[] popOutAnimationsItems;
        [SerializeField] private iTween.EaseType easeType;
        [SerializeField] private float time;
        [SerializeField] private UnityEvent onPopIn;
        private void Start(){
            quitMenu.SetActive(false);
            #if UNITY_ANDROID
            Application.targetFrameRate = 60;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            #endif
            onGameStart?.Invoke();
            AudioManager.current.PlayMusic(SoundType.BGM);
            RefreshCashValue();
            RefreshDimondValue();
            playerData.onDimondValueChange += RefreshDimondValue;
            playerData.onCurrencyValueChange += RefreshCashValue;
            playDemoRenderer.material = playerData.playerSkinMaterial;
            AdController.current.AskinforExtraCoinFromGame(false);
            if(playerData.temporarySkin != null){
                playDemoRenderer.material = playerData.temporarySkin;
            }
            PopOutRoutine();
        }
        private void Update(){
            if(Input.GetKeyDown(KeyCode.Escape)){
                if(!shopMenu.activeInHierarchy && !settingsMenu.activeInHierarchy){
                    quitMenu.SetActive(true);
                }
            }
        }
        public void QuitGame(){
            SavingAndLoadingManager.instance.SaveGame();
            Application.Quit();
        }
        public void PlayGame(){
            AudioManager.current.StopAudio(SoundType.BGM);
            LevelLoader.current.PlayLevel(SceneIndex.Game_Scene);
            AdController.current.AskingforExtraCoinFromShop(false);
            AdController.current.SetTryGetSkinAd(false);
            AdController.current.AskinforExtraCoinFromGame(true);
        }
        public void RefreshCashValue(){
            coinAmount.SetText(string.Concat(playerData.GetCashAmount().ToString()));
            
        }
        public void RefreshDimondValue(){
            dimondAmount.SetText(string.Concat(playerData.GetDimondCount().ToString()));
        }

        private void PopOutRoutine(){
            // for (int i = 0; i < popOutAnimationsItems.Length; i++) {
            //     popOutAnimationsItems[i].PopOut(easeType,time);
            // }
            if(playerData.temporarySkin != null){
                playDemoRenderer.material = playerData.temporarySkin;
            }else{
                playDemoRenderer.material = playerData.playerSkinMaterial;
            }
        }
        // [ContextMenu("Pop Out")]
        // public void PopOut(){
        //     Invoke(nameof(PopOutRoutine),delay);
        // }
        // [ContextMenu("Pop In")]
        // public void PopIn(){
        //     // StartCoroutine(PopInRoutine());
        // }
        // private IEnumerator PopInRoutine(){
        //     for (int i = 0; i < popOutAnimationsItems.Length; i++) {
        //         popOutAnimationsItems[i].PopIn(easeType,time);
        //     }
        //     yield return new WaitForSeconds(time);
        //     onPopIn?.Invoke();

        // }
        
        
        
    }
    [System.Serializable]
    public class PopOutAnimationsItems{
        public GameObject items;
        public GameObject origin;
        public GameObject destination;
        

        public void PopOut(iTween.EaseType easeType,float time){
            iTween.MoveTo(items,iTween.Hash("Position",destination.transform.position,"easyType",easeType,"time",time));
        }
        public void PopIn(iTween.EaseType easeType,float time){
            iTween.MoveTo(items,iTween.Hash("Position",origin.transform.position,"easyType",easeType,"time",time));
            
        }
        

    }
}