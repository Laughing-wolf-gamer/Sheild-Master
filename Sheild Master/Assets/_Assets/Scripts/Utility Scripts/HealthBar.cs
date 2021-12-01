using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
namespace GamerWolf.Utils.HealthSystem {
    public class HealthBar : MonoBehaviour {
        

        [SerializeField] private Transform healthBar;
        [SerializeField] private Image bar;
        [SerializeField] private Canvas canvas;
        private Camera viewCam;
        private void Awake(){
            viewCam =  Camera.main;
        }
        private void Start(){
            canvas.worldCamera = viewCam;
        }

        
        public void UpdateHealthBar(float healthAmount,Transform toLook){
            transform.LookAt(toLook);
            ShowHealthBar();
            bar.fillAmount = healthAmount;
        }
        public void ShowHealthBar(){
            CancelInvoke(nameof(HideHealthBar));
            healthBar.gameObject.SetActive(true);
            Invoke(nameof(HideHealthBar),1f);
        }
        public void HideHealthBar(){
            healthBar.gameObject.SetActive(false);
        }
        
        
    }

}