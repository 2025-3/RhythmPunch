using System.Collections;
using System.Collections.Generic;
using Sheets;
using UnityEngine;

public class JudgeEffectControl : MonoBehaviour
{
    public Sprite perfect;
    public Sprite great;
    public Sprite good;
    public Sprite bad;
    public Sprite miss;

    private SpriteRenderer _renderer;

    public float showTime = 0.1f;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }
    
    private void Start()
    {
        GameManager.Instance.onNoteDestroyed.AddListener((_, judge) =>
        {
            StopAllCoroutines();
            ShowSprite(judge);
            StartCoroutine(RemoveSprite());
        });
    }

    private void ShowSprite(JudgementType type)
    {
        _renderer.sprite = type switch
        {
            JudgementType.Perfect => perfect,
            JudgementType.Great => great,
            JudgementType.Good => good,
            JudgementType.Bad => bad,
            JudgementType.Miss => miss,
            JudgementType.Fail => miss
        };
    }

    private IEnumerator RemoveSprite()
    {
        yield return new WaitForSeconds(showTime);
        _renderer.sprite = null;
    }
}
