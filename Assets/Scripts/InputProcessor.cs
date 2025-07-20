using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InputProcessor : MonoBehaviour
{
    public static InputProcessor Instance;

    public static MoveType currentInput { get; private set; }

    private readonly List<DirectionType> _directions = new();

    private readonly float _resetTime = 0.2f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _directions.Add(DirectionType.Up);
            StopAllCoroutines();
            StartCoroutine(ResetDeque());
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _directions.Add(DirectionType.Down);
            StopAllCoroutines();
            StartCoroutine(ResetDeque());
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _directions.Add(DirectionType.Left);
            StopAllCoroutines();
            StartCoroutine(ResetDeque());
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _directions.Add(DirectionType.Right);
            StopAllCoroutines();
            StartCoroutine(ResetDeque());
        }
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            StopAllCoroutines();

            currentInput = MoveType.High;
            // 노트 정보 저장
            var note = new NoteForJudge
            {
                Time = Time.time,
                Type = MoveType.High,
                Directions = _directions.ToList(), // 복사뜨는거 맘에 안듬
            };

            // 게임매니저에서 판정
            GameManager.Instance.Judge(note);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            StopAllCoroutines();

            currentInput = MoveType.Middle;

            // 노트 정보 저장
            var note = new NoteForJudge
            {
                Time = Time.time,
                Type = MoveType.Middle,
                Directions = _directions.ToList(),
            };
            
            GameManager.Instance.Judge(note);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            StopAllCoroutines();

            currentInput = MoveType.Low;

            // 노트 정보 저장
            var note = new NoteForJudge
            {
                Time = Time.time,
                Type = MoveType.Low,
                Directions = _directions.ToList(),
            };

            GameManager.Instance.Judge(note);
        }
    }

    private IEnumerator ResetDeque()
    {
        yield return new WaitForSeconds(_resetTime);
        _directions.Clear();
    }
}