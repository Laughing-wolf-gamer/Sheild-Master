using UnityEngine;
using UnityEngine.Events;

public class UIWindowAnimaitons : MonoBehaviour {

    
    [SerializeField] private UnityEvent onPlay;

    [ContextMenu("Show window")]
    public void ShowPopUp(){
        onPlay?.Invoke();
    }

}
