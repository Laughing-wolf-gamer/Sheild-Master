using System;
using UnityEngine;

public class TestCrash : MonoBehaviour {

    private int updatesBeforeException = 0;
    private void Start(){
        updatesBeforeException = 0;
    }
    private void Update(){
        throwExceptionEvery60Updates();
    }
    private void throwExceptionEvery60Updates(){
        if (updatesBeforeException > 0){
            updatesBeforeException--;
        }else{
            // Set the counter to 60 updates
            updatesBeforeException = 60;

            // Throw an exception to test your Crashlytics implementation
            throw new Exception("test exception please ignore");
        }
    }
}
