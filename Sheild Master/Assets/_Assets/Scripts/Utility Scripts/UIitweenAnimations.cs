using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIitweenAnimations : MonoBehaviour {

    
    [SerializeField] private Rect objectRect;
    [SerializeField] private Vector3 maxRectPos;
    [SerializeField] private Vector3 maxRectWidth;

    [SerializeField] private float time;
    [SerializeField] private float delay;
    [SerializeField] private iTween.EaseType easeType;
    [SerializeField] private Vector3 currentRectPos;
    [SerializeField] private Vector3 currentRectWidth;
    private void Start(){
        currentRectPos = objectRect.position;
        currentRectWidth = new Vector3(objectRect.width,objectRect.height);
    }
    
    public void PopOut(){
        
    }
}
