using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public static GameManager Instance;

    public NoteType mode;

    public int count = 3;

    public Sheet sheet;

    public JudgementType currentInputJudge;

    public List<Note> commandList;

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
    void Start()
    {
        ChangeMode(NoteType.Attack);
    }

    int index = 0;

    private void Update()
    {
        if (index > 5) return; 

        // 현재 노트의 판정 유효 시간이 지났다면 (Miss 처리)
        if (sheet.sheetData.notes[index].time + 0.5f < Time.time)
        {
            nextNode();

            if (mode == NoteType.Attack)
            {
                // 공격 모드: Miss 상황이므로 강제로 빈 노트를 만들어 기록
                Note note = new Note();
                note.type = (Note.Type)Random.Range(0, 3); // 랜덤 타입 (A, B, C 등)
                note.data = new NoteData();
                note.data.time = Time.time;
                note.data.noteType = GameManager.Instance.mode;
                commandList.Add(note); // 플레이어 입력 목록에 추가
            }
            else
            {
                // 방어 모드: 입력하지 못한 공격을 제거
                if (commandList.Count < 1) return;
                Debug.Log("방어 Miss");
                commandList.RemoveAt(0);
            }
        }
    }
    /// <summary>
    /// 플레이어가 노트를 입력했을 때 호출
    /// </summary>
    /// <param name="note">플레이어 입력 노트</param>
    public void Judge(Note note)
    {
        currentInputJudge = JudgementType.NoJudge;

        // 공격 모드 판정
        if (mode == NoteType.Attack)
        {
            currentInputJudge = sheet.Judge(index, note.data.time);

            //공격 타이밍이 일치하면
            if (currentInputJudge != JudgementType.NoJudge)
            {
                nextNode();
                commandList.Add(note);  //입력 기억하기)

                //HP 소모
            }
        }

        //방어 모드 일시
        else
        {
            //기존 입력 데이터가 없을때
            if (commandList.Count < 1) return;

            // 입력한 노트가 기록된 공격과 다르면 무시           
            if (note.type != commandList[0].type) return;


            currentInputJudge = sheet.Judge(index, note.data.time);

            if (currentInputJudge != JudgementType.NoJudge)
            {
                nextNode();
                commandList.Remove(commandList[0]); // 방어에 성공했으므로 제거

            }

        }

    }

    public void nextNode()
    {
        Debug.Log(index + " : " + currentInputJudge);

        index++;

        if(index > 2 && mode == NoteType.Attack)
        {
            ChangeMode(NoteType.Guard);
        }
    }

    public void ChangeMode(NoteType n)
    {
        mode = n;
        Debug.Log("Mode" + mode);
        if (mode == NoteType.Attack)
        {
            commandList = new List<Note>();
        }
        else
        {
        }
    }
}
