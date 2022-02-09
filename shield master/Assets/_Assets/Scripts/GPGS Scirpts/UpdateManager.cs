using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Google.Play.AppUpdate;
using Google.Play.Common;

public class UpdateManager : MonoBehaviour {

    private AppUpdateManager appUpdateManager;




    private void Start(){
        StartCoroutine(checkForUpdate());
    }
    private IEnumerator checkForUpdate(){
        PlayAsyncOperation<AppUpdateInfo,AppUpdateErrorCode> appUpdateInfoOperation = appUpdateManager.GetAppUpdateInfo();
        // Wait until asynchrounus Operation Complete
        yield return appUpdateInfoOperation;
        if(appUpdateInfoOperation.IsSuccessful){
            var appUpdateInfoResult = appUpdateInfoOperation.GetResult();
            if(appUpdateInfoResult.UpdateAvailability == UpdateAvailability.UpdateAvailable){
                Debug.Log("Update Available");
            }else{
                Debug.Log(" No Update Available");
            }
            var appUpdateOption = AppUpdateOptions.ImmediateAppUpdateOptions();
            yield return StartCoroutine(StartImmediateAppUpdate(appUpdateInfoResult,appUpdateOption));
        }
    }
    private IEnumerator StartImmediateAppUpdate(AppUpdateInfo appUpdateInfo_i,AppUpdateOptions appUpdateOptions_i){
        var startUpdateRequest = appUpdateManager.StartUpdate(appUpdateInfo_i,appUpdateOptions_i);
        yield return startUpdateRequest;
    }
    
}
