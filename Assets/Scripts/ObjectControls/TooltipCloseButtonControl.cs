using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipCloseButtonControl : MonoBehaviour
{
    public GameObject tooltip;
    
    public void OnClick()
    {
        tooltip.SetActive(false);
        gameObject.SetActive(false);
        
        GameManager.Instance.StartGame();
    }
}
