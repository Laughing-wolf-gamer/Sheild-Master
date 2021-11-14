using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace GamerWolf.Utils{
    public class SafeArea : MonoBehaviour {
        
        private RectTransform rectTransform;
        private Rect safeArea;
        Vector2 minAnchor;
        Vector2 maxAnchor;

        private void Awake(){
            rectTransform = GetComponent<RectTransform>();
            safeArea = Screen.safeArea;
            minAnchor = safeArea.position;
            maxAnchor = minAnchor + safeArea.size;


            minAnchor.x /= Screen.width;
            maxAnchor.y /= Screen.height;
            maxAnchor.x /= Screen.width;
            maxAnchor.y /= Screen.height;
            rectTransform.anchorMin = minAnchor;
            rectTransform.anchorMax = maxAnchor;
        
        }
        
    }

}