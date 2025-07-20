using Sheets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPbar : MonoBehaviour
{
    public List<Sprite> sprites = new List<Sprite>();

    public Image backgroundImage;

    private Slider _slider;

    void Start()
    {
        _slider = GetComponent<Slider>();

        GameManager.Instance.onNoteDestroyed.AddListener(UpdateHPbar);

        //체력 게이지에 따라 이미지 변경
        _slider.onValueChanged.AddListener(UpdateBackgroundImage);

        //체력 초기화
        _slider.value = _slider.maxValue;
    }

    private void UpdateHPbar(int index, JudgementType type) //노드 인덱스, 판정 타입
    {
        //BAD, MISS, FAIL 시 체력 소모
        if(type == JudgementType.Fail || type == JudgementType.Miss || type == JudgementType.Bad)
        {
            _slider.value -= 1;
        }
    }

    private void UpdateBackgroundImage(float f)
    {
        //100%일때
        if (f >= _slider.maxValue) backgroundImage.sprite = sprites[sprites.Count - 1];

        //66%일때
        else if (f >= _slider.maxValue / 3 * 2) backgroundImage.sprite = sprites[(sprites.Count - 1)/3 * 2];

        //33%일때
        else if (f >= _slider.maxValue / 3) backgroundImage.sprite = sprites[(sprites.Count - 1) / 3];

        else backgroundImage.sprite = sprites[0];
    }

}
