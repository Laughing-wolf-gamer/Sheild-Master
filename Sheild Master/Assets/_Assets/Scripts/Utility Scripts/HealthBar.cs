using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
namespace GamerWolf.Utils.HealthSystem {
    public class HealthBar : MonoBehaviour {
        

        [SerializeField] private Transform healthBar;
        [SerializeField] private Image bar;
        [SerializeField] private Canvas canvas;
        private Camera viewCam;
        
        
        private void Start(){
            canvas.worldCamera = viewCam;
            healthBar.gameObject.SetActive(true);
            bar.fillAmount = 1f;
            transform.LookAt(viewCam.transform);
        }
        private void LateUpdate(){
            if(viewCam != null){
                transform.LookAt(viewCam.transform);
            }

        }
        
        public void UpdateHealthBar(float healthAmount){
            // ShowHealthBar();
            bar.fillAmount = healthAmount;
        }
        public void ShowHealthBar(){
            // CancelInvoke(nameof(HideHealthBar));
            healthBar.gameObject.SetActive(true);
            // Invoke(nameof(HideHealthBar),1f);
        }
        public void HideHealthBar(){
            healthBar.gameObject.SetActive(false);
        }
        public void SetworldCamera(Camera viewCamera){
            viewCam = viewCamera;
        }
        
        
    }

}