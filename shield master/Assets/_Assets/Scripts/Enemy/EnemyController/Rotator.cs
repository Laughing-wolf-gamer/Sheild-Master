using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SheildMaster{
    public enum RotationAxis{
        Vertical,Horizontal,localVeritcal,localHorizontal,
    }
    public class Rotator : MonoBehaviour {
        
        [SerializeField] private RotationAxis rotationAxis;


        public void Rotate(Transform target){
            Vector3 dir = (target.position - transform.position).normalized;
            float angle = Mathf.Atan2(dir.x,dir.z) * Mathf.Rad2Deg;
            switch(rotationAxis){
                case RotationAxis.Vertical:
                    transform.rotation = Quaternion.Euler(0f,angle,0f);

                break;
                case RotationAxis.localHorizontal:
                    // float newAngle = Mathf.Atan2(dir.x,dir.z) * Mathf.Rad2Deg;
                    transform.LookAt(target.localPosition);
                    
                break;
            }
        }
        
    }

}