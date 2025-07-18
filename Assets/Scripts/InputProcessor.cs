using UnityEngine;

public class InputProcessor : MonoBehaviour
{
    public static InputProcessor Instance;

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
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // 노트 정보 저장
            var note = new NoteForJudge
            {
                Time = Time.time,
                Type = MoveType.High,
            };

            // 게임매니저에서 판정
            GameManager.Instance.Judge(note);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            // 노트 정보 저장
            var note = new NoteForJudge
            {
                Time = Time.time,
                Type = MoveType.Middle,
            };
            
            GameManager.Instance.Judge(note);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            // 노트 정보 저장
            var note = new NoteForJudge
            {
                Time = Time.time,
                Type = MoveType.Low,
            };

            GameManager.Instance.Judge(note);
        }
    }
}