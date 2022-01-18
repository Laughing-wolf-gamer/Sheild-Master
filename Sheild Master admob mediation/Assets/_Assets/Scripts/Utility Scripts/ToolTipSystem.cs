using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
namespace GamerWolf.Utils{
    public class ToolTipSystem : MonoBehaviour{

        public static ToolTipSystem Instance {get;private set;}
        [SerializeField] public Image bgImage;
        [SerializeField] private RectTransform parentCanvas;
        [SerializeField] private Color toolTipColor;
        [SerializeField] private Vector2 padding;
        private RectTransform backGroundTransfom;

        private TextMeshProUGUI toolTiptext;
        private RectTransform currentRectTransform;
        private Func<string> getToolTipFunc;
        
        private float showToolTipTimer;
        private float flashTimer;
        private int flashState;
        
        private void Awake(){
            Instance = this;
            backGroundTransfom = transform.Find("bg").GetComponent<RectTransform>();
            toolTiptext = transform.Find("tool Tip Text").GetComponent<TextMeshProUGUI>();
            currentRectTransform = GetComponent<RectTransform>();
            // parentCanvas = transform.parent.transform.parent.GetComponent<RectTransform>();
            HideToolTip();
        }
        
        private void SetText(string _toolTipText){
            
            toolTiptext.SetText(_toolTipText);
            toolTiptext.ForceMeshUpdate();
            Vector2 textSize = toolTiptext.GetRenderedValues(false) ;
            backGroundTransfom.sizeDelta = textSize + padding;
            
        }
        private void Update(){
            SetText(getToolTipFunc());
            
            
            Vector2 ancoredPos = Input.mousePosition / parentCanvas.localScale.x;
            if(ancoredPos.x + backGroundTransfom.rect.width > parentCanvas.rect.width){
                ancoredPos.x = parentCanvas.rect.width - backGroundTransfom.rect.width;
            }
            if(ancoredPos.y + backGroundTransfom.rect.height > parentCanvas.rect.height){
                ancoredPos.y = parentCanvas.rect.height - backGroundTransfom.rect.height;
            }
            currentRectTransform.anchoredPosition = ancoredPos;
            flashTimer += Time.deltaTime;
            float flashTimerMax = 0.05f;
            if(flashTimer >= flashTimerMax){
                flashState++;
                switch (flashState){
                    case 1:
                    case 3:
                    case 5:
                        toolTiptext.color = new Color(1f,1f,1f,1); 
                        // bgImage.color = new Color(178f/255f,8/255f,0/255f,1);
                        bgImage.color = toolTipColor;
                    break;
                    case 2:
                    case 4:
                        toolTiptext.color = new Color(178f/255f,8/255f,0/255f,1);
                        // bgImage.color = new Color(1f,1f,1f,1); 
                        bgImage.color = toolTipColor;

                    break;
                    


                    
                }
            }

            showToolTipTimer -= Time.deltaTime;
            if(showToolTipTimer <= 0f){
                HideToolTip();
            }
            
        }
        private void ShowToolTip(string toolTipText,Color toolTipColor,float showtimerMax = 2f){
            ShowToolTip(() => toolTipText,toolTipColor,showtimerMax);
            
        }
        private void ShowToolTip(Func<string> GetToolTipTextFunc,Color toolTipColor,float maxShowToolTipTimer = 2f){
            gameObject.SetActive(true);
            this.toolTipColor = toolTipColor;
            this.getToolTipFunc = GetToolTipTextFunc;
            showToolTipTimer = maxShowToolTipTimer;
            flashTimer = 0f;
            flashState = 0;
            SetText(getToolTipFunc());

        }
        private void HideToolTip(){
            gameObject.SetActive(false);
        }
        public static void showToolTip_static(string _toolTipText,Color toolTipColor){
            Instance.ShowToolTip(_toolTipText,toolTipColor);
        }
        public static void showToolTip_static(Func<string> toolTipTextFunc,Color toolTipcolor){
            Instance.ShowToolTip(toolTipTextFunc,toolTipcolor);
            
        }
        public static void HideToolTip_static(){
            Instance.HideToolTip();
        }
    }

}