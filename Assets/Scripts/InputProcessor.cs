using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputProcessor : MonoBehaviour
{

    public int count = 3;

    public static InputProcessor instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //노트 정보 저장
            Note note = new Note();
            note.type = Note.Type.top;
            note.data = new NoteData();
            note.data.time = Time.time;
            note.data.noteType = GameManager.Instance.mode;

            //게임 매니저 판단
            GameManager.Instance.Judge(note);
         
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            Note note = new Note();
            note.type = Note.Type.middle;
            note.data = new NoteData();
            note.data.time = Time.time;
            note.data.noteType = GameManager.Instance.mode;


            GameManager.Instance.Judge(note);

        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Note note = new Note();
            note.type = Note.Type.bottom;
            note.data = new NoteData();
            note.data.time = Time.time;
            note.data.noteType = GameManager.Instance.mode;

            GameManager.Instance.Judge(note);

        }
        
    }
}

public class Note
{
    public NoteData data;
    public enum Type { top, middle, bottom, NaN }
    public Type type;
}