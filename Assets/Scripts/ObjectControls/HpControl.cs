using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpControl : MonoBehaviour
{
    public int index;
    
    public Sprite full;
    public Sprite empty;

    private SpriteRenderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        _renderer.sprite = GameManager.Instance.NowHp > index ? full : empty;
    }
}
