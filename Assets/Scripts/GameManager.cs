using System;
using System.Collections;
using System.Collections.Generic;
using Sheets;
using UnityEngine;
using UnityEngine.Events;

using CounterList = System.Collections.Generic.List<(System.Collections.Generic.List<DirectionType> directions, MoveType moveType, bool isUsed)>;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // 싱글톤

    public NoteType Mode { get; private set; }
    public int NowModeCount { get; private set; } = 0; // 현재 모드의 몇번째인지
    public int NowModeLength { get; private set; }= 1; // 현재 모드 개수
    
    private bool _isPlaying = false; // 플레잉중인지
    private int _noteIndex = 0; // 현재 판정할 노트 인덱스
    private float _startTime; // 시작된 타임스탬프
    private float CurrentTime => Time.time - _startTime; // 타임스탬프 기반 시작된지 몇초지났는지
    
    public List<Sheet> sheets; // 악보정보
    public int SheetIndex { get; private set; } = 0;
    public int level = 1;
    
    public readonly Queue<MoveType> CommandList = new(); // 공격모드때 쌓인 커맨드 리스트
    public readonly CounterList CounterList = new(); // 방어모드때 해야할 카운터 리스트
    private GenerateCounterListsStrategy _counterGenerator;

    public int ComboCount { get; private set; } = 0;
    
    public UnityEvent onStartGame; // 게임 시작 시 발생
    public UnityEvent onEndGame; // 게임 종료 시 발생
    public UnityEvent<int, JudgementType> onNoteDestroyed; // 노트 파괴 (시간초과 or 판정) 시 발생
    public UnityEvent<int, JudgementType, NoteForJudge> onNoteDestroyedWithNote;
    public UnityEvent<MoveType> onComboAdded;
    public UnityEvent onCounterChanged;
    
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
        level = sheets[0].sheetData.level;
        _counterGenerator = level switch
        {
            0 => new Stage0(),
            1 => new Stage1(),
            2 => new Stage2(),
            3 => new Stage3(),
            4 => new Stage4(),
            5 => new Stage5(),
            6 => new Stage6()
        };
        
        StartCoroutine(DelayedStart()); // 적절한 위치로 옮겨야함 (ui라던가 onLoad라던가 등등)
    }

    private IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(3.0f);
        StartGame();
    }

    private void Update()
    {
        if (_isPlaying)
        {
            if (_noteIndex >= sheets[SheetIndex].sheetData.notes.Length)
            {
                ChangeSheet();
            }
            
            MissJudge(); // 현재 노트의 판정 유효 시간이 지났다면 (Miss 처리)
        }
    }

    public void StartGame()
    {
        _isPlaying = true;
        _startTime = Time.time;
        ChangeMode(sheets[0].sheetData.notes[0].noteType);
        onStartGame?.Invoke();
        SoundManager.Instance.PlayBGM(0, true);
    }

    public void EndGame()
    {
        _isPlaying = false;
        onEndGame?.Invoke();
    }

    private void MissJudge()
    {
        // 현재 노트의 판정 유효 시간이 지났다면 (Miss 처리)
        if (sheets[SheetIndex].sheetData.notes[_noteIndex].time + 0.5f < CurrentTime)
        {
            if (Mode == NoteType.Attack)
            {
                // 공격 모드: Miss 상황이므로 강제로 빈 노트를 만들어 기록
                var randomNote = (MoveType)Random.Range(0, 3);
                CommandList.Enqueue(randomNote); // 플레이어 입력 목록에 랜덤값 추가
                onComboAdded?.Invoke(randomNote);
            }
            else
            {
                // 방어 모드: 입력하지 못한 공격을 제거
                Debug.Log("방어 Miss");
                CommandList.Dequeue();
            }
            
            onNoteDestroyedWithNote?.Invoke(_noteIndex, JudgementType.Miss, null);
            NextNode(JudgementType.Miss);
        }
    }
    
    public void Judge(NoteForJudge note)
    {
        var noteRealTime = note.Time - _startTime;

        if (Mode == NoteType.Attack) // 공격 모드 판정
        {
            var currentInputJudge = sheets[SheetIndex].Judge(_noteIndex, noteRealTime);

            if (currentInputJudge != JudgementType.NoJudge) // 공격 타이밍이 일치하면
            {
                Debug.Log(currentInputJudge);
                CommandList.Enqueue(note.Type);  // 입력 기억하기
                onComboAdded?.Invoke(note.Type);
                
                onNoteDestroyedWithNote?.Invoke(_noteIndex, currentInputJudge, note);
                NextNode(currentInputJudge);
                
                //HP 소모
            }
        }
        else // 방어 모드 일시
        {
            var currentInputJudge = sheets[SheetIndex].Judge(_noteIndex, noteRealTime);
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
                
                onNoteDestroyedWithNote?.Invoke(_noteIndex, currentInputJudge, note);
                NextNode(currentInputJudge);
            }
        }
    }

    private void NextNode(JudgementType reason)
    {
        onNoteDestroyed?.Invoke(_noteIndex, reason);

        if (reason is JudgementType.Perfect or JudgementType.Great or JudgementType.Good)
        {
            ComboCount++;
        }
        else
        {
            ComboCount = 0;
        }
        
        _noteIndex++;
        NowModeCount++;
        
        if (NowModeCount >= NowModeLength)
            ChangeMode(Mode == NoteType.Attack ? NoteType.Guard : NoteType.Attack);
        
        Debug.Log(_noteIndex);
    }

    private void ChangeMode(NoteType n)
    {
        Mode = n;

        NowModeCount = 0;
        NowModeLength = 0;
        while (_noteIndex + NowModeLength < sheets[SheetIndex].sheetData.notes.Length &&
               Mode == sheets[SheetIndex].sheetData.notes[_noteIndex + NowModeLength].noteType)
        {
            NowModeLength++;
        }

        Debug.Log("Mode" + Mode);
        
        if (Mode == NoteType.Attack)
        {
            CommandList.Clear();
            GenerateCounterList();
        }
    }

    private void GenerateCounterList()
    {
        CounterList.Clear();
        var counterList = _counterGenerator.Generate();

        foreach (var item in counterList)
        {
            CounterList.Add((item, (MoveType)Random.Range(0, 3), false));
        }
        
        foreach (var item in CounterList)
            Debug.Log(string.Join("", item.directions) + ": " + item.moveType);

        onCounterChanged?.Invoke();
    }

    private bool CheckCounterList(NoteForJudge note)
    {
        for (int i = 0; i < CounterList.Count; i++)
        {
            var (directionList, moveType, isUsed) = CounterList[i];
            if (!isUsed && note.Type == moveType && note.Directions.Count >= directionList.Count)
            {
                bool isSame = true;
                for (int j = 0; j < directionList.Count; j++)
                {
                    if (directionList[i] != note.Directions[^(j+1)])
                    {
                        isSame = false;
                        break;
                    }
                }

                if (isSame)
                {
                    CounterList[i] = (directionList, moveType, true);
                    onCounterChanged?.Invoke();
                    return true;
                }
            }
        }
        return false;
    }

    private void ChangeSheet()
    {
        SheetIndex++;
        Debug.Log($"Sheet {SheetIndex}");
        if (SheetIndex >= sheets.Count)
        {
            EndGame();
            return;
        }

        _noteIndex = 0;
        level = sheets[SheetIndex].sheetData.level;
        _counterGenerator = level switch
        {
            0 => new Stage0(),
            1 => new Stage1(),
            2 => new Stage2(),
            3 => new Stage3(),
            4 => new Stage4(),
            5 => new Stage5(),
            6 => new Stage6()
        };
        
        ChangeMode(sheets[SheetIndex].sheetData.notes[0].noteType);
    }
}
