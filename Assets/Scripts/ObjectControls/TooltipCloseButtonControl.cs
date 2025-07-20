using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipCloseButtonControl : MonoBehaviour
{
    public GameObject tooltip;

    public GameObject winPanel;
    public GameObject losePanel;
    
    private void Awake()
    {
        GameManager.Instance.onWinGame.AddListener(() =>
        {
            winPanel.SetActive(true);
        });
        
        GameManager.Instance.onEndGame.AddListener(() =>
        {
            losePanel.SetActive(true);
        });
    }
    
    public void OnClick()
    {
        tooltip.SetActive(false);
        gameObject.SetActive(false);
        
        GameManager.Instance.StartGame();
    }
}
