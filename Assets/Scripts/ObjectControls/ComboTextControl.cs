using TMPro;
using UnityEngine;

namespace ObjectControls
{
    public class ComboTextControl : MonoBehaviour
    {
        private TextMeshProUGUI _text;

        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
        }
    
        private void Update()
        {
            _text.text = GameManager.Instance.ComboCount == 0 ? "" : $"{GameManager.Instance.ComboCount}";
        }
    }
}
