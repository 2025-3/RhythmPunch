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

        //ü�� �������� ���� �̹��� ����
        _slider.onValueChanged.AddListener(UpdateBackgroundImage);

        //ü�� �ʱ�ȭ
        _slider.value = _slider.maxValue;
    }

    private void UpdateHPbar(int index, JudgementType type) //��� �ε���, ���� Ÿ��
    {
        //BAD, MISS, FAIL �� ü�� �Ҹ�
        if(type == JudgementType.Fail || type == JudgementType.Miss || type == JudgementType.Bad)
        {
            _slider.value -= 1;
        }
    }

    private void UpdateBackgroundImage(float f)
    {
        //100%�϶�
        if (f >= _slider.maxValue) backgroundImage.sprite = sprites[sprites.Count - 1];

        //66%�϶�
        else if (f >= _slider.maxValue / 3 * 2) backgroundImage.sprite = sprites[(sprites.Count - 1)/3 * 2];

        //33%�϶�
        else if (f >= _slider.maxValue / 3) backgroundImage.sprite = sprites[(sprites.Count - 1) / 3];

        else backgroundImage.sprite = sprites[0];
    }

}
