using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace ObjectControls
{
    public class TooltipCloseButtonControl : MonoBehaviour
    {
        public GameObject tooltip;
        
        private TextMeshProUGUI _text;
        private bool _isSheetLoaded = false;

        private void Awake()
        {
            _text = GetComponentInChildren<TextMeshProUGUI>();
            
            Assert.IsNotNull(_text);
        }
        
        private void Start()
        {
            GameManager.Instance.onCompleteLoadSheet.AddListener(SetClickable);
        }

        private void SetClickable()
        {
            GameManager.Instance.onCompleteLoadSheet.RemoveListener(SetClickable);
            
            _text.text = "Click Anywhere To Start";
            _isSheetLoaded = true;
        }
        
        public void OnClick()
        {
            if (!_isSheetLoaded)
                return;
            
            tooltip.SetActive(false);
            gameObject.SetActive(false);
        
            GameManager.Instance.StartGame();
        }
    }
}
