using System;
using System.Collections.Generic;
using Sheets;
using UnityEngine;
using UnityEngine.Events;

using CounterList = System.Collections.Generic.List<(System.Collections.Generic.List<DirectionType> directions, MoveType moveType, bool isUsed)>;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // 싱글톤

    private NoteType _mode; // 공격모드 방어모드 정보
    private int _nowModeCount = 0; // 현재 모드의 몇번째인지
    private int _nowModeLength = 1; // 현재 모드 개수
    
    private bool _isPlaying = false; // 플레잉중인지
    private int _noteIndex = 0; // 현재 판정할 노트 인덱스
    private float _startTime; // 시작된 타임스탬프
    private float CurrentTime => Time.time - _startTime; // 타임스탬프 기반 시작된지 몇초지났는지
    
    public Sheet sheet; // 악보정보
    public int level;
    
    public readonly Queue<MoveType> CommandList = new(); // 공격모드때 쌓인 커맨드 리스트
    public readonly CounterList CounterList = new(); // 방어모드때 해야할 카운터 리스트
    
    public UnityEvent onStartGame; // 게임 시작 시 발생
    public UnityEvent onEndGame; // 게임 종료 시 발생
    public UnityEvent<int, JudgementType> onNoteDestroyed; // 노트 파괴 (시간초과 or 판정) 시 발생
    
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
    
    private void Start()
    {
        StartGame(); // 적절한 위치로 옮겨야함 (ui라던가 onLoad라던가 등등)
    }

    private void Update()
    {
        if (_isPlaying)
        {
            if (_noteIndex >= sheet.sheetData.notes.Length)
            {
                EndGame();
                return;
            }
            
            MissJudge(); // 현재 노트의 판정 유효 시간이 지났다면 (Miss 처리)
        }
    }

    public void StartGame()
    {
        _isPlaying = true;
        _startTime = Time.time;
        ChangeMode(sheet.sheetData.notes[0].noteType);
        onStartGame?.Invoke();
    }

    public void EndGame()
    {
        _isPlaying = false;
        onEndGame?.Invoke();
    }

    private void MissJudge()
    {
        // 현재 노트의 판정 유효 시간이 지났다면 (Miss 처리)
        if (sheet.sheetData.notes[_noteIndex].time + 0.5f < CurrentTime)
        {
            if (_mode == NoteType.Attack)
            {
                // 공격 모드: Miss 상황이므로 강제로 빈 노트를 만들어 기록
                CommandList.Enqueue((MoveType)Random.Range(0, 3)); // 플레이어 입력 목록에 랜덤값 추가
            }
            else
            {
                // 방어 모드: 입력하지 못한 공격을 제거
                Debug.Log("방어 Miss");
                CommandList.Dequeue();
            }
            NextNode(JudgementType.Miss);
        }
    }
    
    public void Judge(NoteForJudge note)
    {
        var noteRealTime = note.Time - _startTime;

        if (_mode == NoteType.Attack) // 공격 모드 판정
        {
            var currentInputJudge = sheet.Judge(_noteIndex, noteRealTime);

            if (currentInputJudge != JudgementType.NoJudge) // 공격 타이밍이 일치하면
            {
                Debug.Log(currentInputJudge);
                CommandList.Enqueue(note.Type);  // 입력 기억하기
                NextNode(currentInputJudge);
                
                //HP 소모
            }
        }
        else // 방어 모드 일시
        {
            var currentInputJudge = sheet.Judge(_noteIndex, noteRealTime);
            if (currentInputJudge != JudgementType.NoJudge)
            {
                Debug.Log(CommandList.Peek());
                // 입력한 노트가 기록된 공격과 다르면 실패처리          
                if (note.Type != CommandList.Peek())
                    currentInputJudge = JudgementType.Fail;
                
                // 여기서 카운터 처리 필요
                if (CheckCounterList(note))
                {
                    
                }
                
                Debug.Log(currentInputJudge);
                
                CommandList.Dequeue(); // 방어에 성공했으므로 제거
                NextNode(currentInputJudge);
            }
        }
    }

    private void NextNode(JudgementType reason)
    {
        onNoteDestroyed?.Invoke(_noteIndex, reason);
        
        _noteIndex++;
        _nowModeCount++;
        
        if (_nowModeCount >= _nowModeLength)
            ChangeMode(_mode == NoteType.Attack ? NoteType.Guard : NoteType.Attack);
    }

    private void ChangeMode(NoteType n)
    {
        _mode = n;

        _nowModeCount = 0;
        _nowModeLength = 0;
        while (_noteIndex + _nowModeLength < sheet.sheetData.notes.Length &&
               _mode == sheet.sheetData.notes[_noteIndex + _nowModeLength].noteType)
        {
            _nowModeLength++;
        }

        Debug.Log("Mode" + _mode);
        
        if (_mode == NoteType.Attack)
        {
            CommandList.Clear();
            GenerateCounterList();
        }
    }

    private void GenerateCounterList()
    {
        CounterList.Clear();
        for (int i = 0; i < 3; i++)
        {
            List<DirectionType> directions = new();
            bool needGenerate = true;
            while (needGenerate)
            {
                needGenerate = false;
                for (int j = 0; j < level; j++)
                {
                    directions.Add((DirectionType)Random.Range(0, 4));
                    
                    if (j >= 1 &&
                        ((directions[j - 1] == DirectionType.Up && directions[j] == DirectionType.Up) ||
                         (directions[j - 1] == DirectionType.Down && directions[j] == DirectionType.Down)))
                    {
                        needGenerate = true;
                        directions.Clear();
                        break;
                    }
                }
            }
            
            CounterList.Add((directions, (MoveType)Random.Range(0, 3), false));
        }
    }

    private bool CheckCounterList(NoteForJudge note)
    {
        for (int i = 0; i < CounterList.Count; i++)
        {
            var (directionList, moveType, isUsed) =  CounterList[i];
            if (!isUsed && note.Type == moveType && note.Directions.Count >= directionList.Count)
            {
                bool isSame = true;
                for (int j = 0; j < directionList.Count; j++)
                {
                    if (directionList[i] != note.Directions[^j])
                    {
                        isSame = false;
                        break;
                    }
                }

                if (isSame)
                {
                    CounterList[i] = (directionList, moveType, true);
                    return true;
                }
            }
        }
        return false;
    }
}
