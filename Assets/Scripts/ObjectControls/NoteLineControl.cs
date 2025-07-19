using System.Collections;
using System.Collections.Generic;
using Sheets;
using UnityEngine;

public class NoteLineControl : MonoBehaviour
{
    private Sprite _normalBar;
    public Sprite highBar;
    public Sprite middleBar;
    public Sprite lowBar;
    
    private SpriteRenderer _spriteRenderer;

    private float _changeTime = 0.3f;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _normalBar = _spriteRenderer.sprite;
    }

    private void Start()
    {
        GameManager.Instance.onNoteDestroyedWithNote.AddListener((_, judge, note) =>
        {
            StopAllCoroutines();
            StartCoroutine(ChangeBar(judge, note));
        });    
    }

    private IEnumerator ChangeBar(JudgementType judge, NoteForJudge note)
    {
        if (judge is JudgementType.Miss or JudgementType.Bad or JudgementType.Fail)
            yield break;
        
        _spriteRenderer.sprite = note.Type switch
        {
            MoveType.High => highBar,
            MoveType.Middle => middleBar,
            MoveType.Low => lowBar,
        };
        
        yield return new WaitForSeconds(_changeTime);
        
        _spriteRenderer.sprite = _normalBar;
    }
}
