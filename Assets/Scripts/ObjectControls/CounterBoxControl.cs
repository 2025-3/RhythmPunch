using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterBoxControl : MonoBehaviour
{
    public int index;

    public Sprite upArrow;
    public Sprite downArrow;
    public Sprite leftArrow;
    public Sprite rightArrow;

    public Sprite highNote;
    public Sprite middleNote;
    public Sprite lowNote;

    private SpriteRenderer[][] _spriteRenderer = new SpriteRenderer[5][];

    private void Awake()
    {
        for (int i = 2; i <= 4; i++)
        {
            _spriteRenderer[i] = new SpriteRenderer[i];
            for (int j = 0; j < i; j++)
            {
                var childName = $"renderer{i}{j}";
                _spriteRenderer[i][j] = transform.Find(childName).GetComponent<SpriteRenderer>();
            }
        }
    }
    
    // Start is called before the first frame update
    private void Start()
    {
        GameManager.Instance.onCounterChanged.AddListener(Draw);
        StartCoroutine(PlayAnimation());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Draw()
    {
        RemoveAllSprites();
        
        if (index >= GameManager.Instance.CounterList.Count)
        {
            gameObject.SetActive(false);
            return;
        }
        
        var (directions, moveType, isUsed) = GameManager.Instance.CounterList[index];

        if (isUsed)
        {
            gameObject.SetActive(false);
            return;
        }
        
        gameObject.SetActive(true);

        int length = directions.Count + 1;

        for (int i = 0; i < length - 1; i++)
        {
            _spriteRenderer[length][i].sprite = directions[i] switch
            {
                DirectionType.Up => upArrow,
                DirectionType.Down => downArrow,
                DirectionType.Left => leftArrow,
                DirectionType.Right => rightArrow,
            };
        }

        _spriteRenderer[length][length - 1].sprite = moveType switch
        {
            MoveType.High => highNote,
            MoveType.Middle => middleNote,
            MoveType.Low => lowNote,
        };
    }

    private void RemoveAllSprites()
    {
        for (int i = 2; i <= 4; i++)
        {
            for (int j = 0; j < i; j++)
            {
                _spriteRenderer[i][j].sprite = null;
            }
        }
    }

    private IEnumerator PlayAnimation()
    {
        float perdiod = 0.5f;
        float distance = 0.1f;
        float direction = -1.0f;
        while (true)
        {
            yield return new WaitForSeconds(perdiod);
            
            
            transform.position += direction * distance * (index == 2 ? Vector3.left : Vector3.up);
            direction = -direction;
        }
    }
}
