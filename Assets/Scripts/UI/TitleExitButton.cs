using UnityEngine;

namespace UI
{
    public class TitleExitButton : MonoBehaviour
    {
        private void Awake()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            gameObject.SetActive(false);
#endif
        }
    }
}