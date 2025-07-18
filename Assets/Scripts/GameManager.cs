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

        // ���� ��Ʈ�� ���� ��ȿ �ð��� �����ٸ� (Miss ó��)
        if (sheet.sheetData.notes[index].time + 0.5f < Time.time)
        {
            nextNode();

            if (mode == NoteType.Attack)
            {
                // ���� ���: Miss ��Ȳ�̹Ƿ� ������ �� ��Ʈ�� ����� ���
                Note note = new Note();
                note.type = (Note.Type)Random.Range(0, 3); // ���� Ÿ�� (A, B, C ��)
                note.data = new NoteData();
                note.data.time = Time.time;
                note.data.noteType = GameManager.Instance.mode;
                commandList.Add(note); // �÷��̾� �Է� ��Ͽ� �߰�
            }
            else
            {
                // ��� ���: �Է����� ���� ������ ����
                if (commandList.Count < 1) return;
                Debug.Log("��� Miss");
                commandList.RemoveAt(0);
            }
        }
    }
    /// <summary>
    /// �÷��̾ ��Ʈ�� �Է����� �� ȣ��
    /// </summary>
    /// <param name="note">�÷��̾� �Է� ��Ʈ</param>
    public void Judge(Note note)
    {
        currentInputJudge = JudgementType.NoJudge;

        // ���� ��� ����
        if (mode == NoteType.Attack)
        {
            currentInputJudge = sheet.Judge(index, note.data.time);

            //���� Ÿ�̹��� ��ġ�ϸ�
            if (currentInputJudge != JudgementType.NoJudge)
            {
                nextNode();
                commandList.Add(note);  //�Է� ����ϱ�)

                //HP �Ҹ�
            }
        }

        //��� ��� �Ͻ�
        else
        {
            //���� �Է� �����Ͱ� ������
            if (commandList.Count < 1) return;

            // �Է��� ��Ʈ�� ��ϵ� ���ݰ� �ٸ��� ����           
            if (note.type != commandList[0].type) return;


            currentInputJudge = sheet.Judge(index, note.data.time);

            if (currentInputJudge != JudgementType.NoJudge)
            {
                nextNode();
                commandList.Remove(commandList[0]); // �� ���������Ƿ� ����

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
