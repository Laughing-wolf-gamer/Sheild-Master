using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIWindowAnimaitons : MonoBehaviour {

    public GameObject objectToMove;
    public GameObject destination;
    public Vector3 offset;
    public float timetomove;
    public float dealy;
    public iTween.EaseType easeType;
    [ContextMenu("Show window")]
    public void ShowPopUp(){
        Invoke(nameof(ShowWindow),dealy);
    }
    public void ShowWindow(){
        iTween.MoveTo(objectToMove,iTween.Hash("Position",destination.transform.position + offset,"easyType",easeType,"time",timetomove));
        CancelInvoke(nameof(OnAfterAnimations));
        Invoke(nameof(OnAfterAnimations),timetomove + 1);
    }
    private void OnAfterAnimations(){
        objectToMove.transform.SetParent(destination.transform);
    }
}
