using UnityEngine;
using GamerWolf.Utils;
using System.Collections;
using UnityEngine.Events;
namespace GamerWolf.Utils{
    public class UICoinParitcalEffect : MonoBehaviour {
        
        [SerializeField] private bool isCoinParitcal = true;
        [SerializeField] private GameObject origin;
        [SerializeField] private GameObject destination;
        [SerializeField] private iTween.EaseType easeType;
        [SerializeField] private float time;
        [SerializeField] private float rate;
        [SerializeField] private float amount;
        [SerializeField] private Vector3 offset;
        [SerializeField] private UnityEvent onEffectComplete;
        private ObjectPoolingManager poolingManager;
        private void Start(){
            poolingManager = ObjectPoolingManager.current;
        }
        public void FromTo(){
            StartCoroutine(FromToRoutine());
        }
        private IEnumerator FromToRoutine(){
            for (int i = 0; i < amount; i++){
                GameObject vfx = poolingManager.SpawnFromPool((isCoinParitcal ? PoolObjectTag.cashUIParitical : PoolObjectTag.DimondUiPartical),origin.transform.position,Quaternion.identity) as GameObject;
                PooledObject poolObject = vfx.GetComponent<PooledObject>();
                poolObject.transform.SetParent(origin.transform);
                iTween.MoveTo(poolObject.gameObject,iTween.Hash("Position",destination.gameObject.transform.position + offset,"easyType",easeType,"time",time));
                poolObject.DestroyMySelf(time + 1);
                yield return new WaitForSeconds(rate);
            }
            onEffectComplete?.Invoke();
        }
    }
}