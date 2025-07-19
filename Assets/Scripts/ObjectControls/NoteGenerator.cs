using System.Collections.Generic;
using Sheets;
using UnityEngine;

namespace ObjectControls
{
    public class NoteGenerator : MonoBehaviour
    {
        public NoteControl note;

        public Sprite guardSprite;
        public Sprite highSprite;
        public Sprite middleSprite;
        public Sprite lowSprite;
        
        public Vector2 startPos;
        public Vector2 endPos;
        public float z;
        public float moveTime;

        private readonly Queue<NoteControl> _noteQueue = new();
        private readonly Queue<GameObject> _moveNoteQueue = new();
        private readonly Queue<GameObject> _guardNotes = new();
        
        private List<MoveType> _moveTypeQueue = new();

        private bool _isStart = false;
        private int _noteIndex = 0;
        private float _startTime;
        private float CurrentTime => Time.time - _startTime;

        private List<Sheet> sheets => GameManager.Instance.sheets;
        private int _sheetIndex = 0;
        
        private void Awake()
        {
            
        }

        private void Start()
        {
            moveTime = sheets[0].sheetData.reachingTime;
            
            GameManager.Instance.onStartGame.AddListener(StartGenerate);
            GameManager.Instance.onEndGame.AddListener(EndGenerate);
            GameManager.Instance.onNoteDestroyed.AddListener((_, reason) => { DestroyNote(reason); });
            GameManager.Instance.onComboAdded.AddListener(ChangeGuardNote);

            foreach (var sheet in sheets)
            {
                for (int i = 0; i < sheet.sheetData.notes.Length; i++)
                {
                    var initPos = new Vector3(startPos.x, startPos.y, z);
                    var go = Instantiate(note, initPos, Quaternion.identity);
                    go.gameObject.SetActive(false);
                    _noteQueue.Enqueue(go);
                    if (sheet.sheetData.notes[i].noteType == NoteType.Guard)
                    {
                        go.GetComponent<SpriteRenderer>().sprite = guardSprite;
                        _guardNotes.Enqueue(go.gameObject);
                    }
                }
            }
        }

        private void Update()
        {
            if (_isStart)
            {
                if (_noteIndex >= sheets[_sheetIndex].sheetData.notes.Length)
                {
                    ChangeSheet();
                }
                else if (CurrentTime >= sheets[_sheetIndex].sheetData.notes[_noteIndex].time - moveTime)
                {
                    GenerateNote();
                    _noteIndex++;
                }
            }
        }
        
        private void StartGenerate()
        {
            _isStart = true;
            _startTime = Time.time;
        }

        private void EndGenerate()
        {
            _isStart = false;
            while (_noteQueue.Count > 0)
            {
                var go = _noteQueue.Dequeue().gameObject;
                Destroy(go);
            }

            while (_moveNoteQueue.Count > 0)
            {
                var go = _moveNoteQueue.Dequeue();
                Destroy(go);
            }
        }
        
        private void GenerateNote()
        {
            var newNote = _noteQueue.Dequeue();
            
            newNote.startPos = startPos;
            newNote.endPos = endPos;
            newNote.moveTime = moveTime;

            newNote.gameObject.SetActive(true);
            newNote.StartMove();
            
            _moveNoteQueue.Enqueue(newNote.gameObject);
        }

        private void DestroyNote(JudgementType reason)
        {
            var oldNote = _moveNoteQueue.Dequeue();
            
            // need to generate particle
            switch (reason)
            {
                case JudgementType.Miss:
                case JudgementType.Fail:
                    break;
                
                case JudgementType.Good:
                    break;
                
                case JudgementType.Perfect:
                    break;
            }
            
            Destroy(oldNote);
        }

        private void ChangeGuardNote(MoveType type)
        {
            var guardNote = _guardNotes.Dequeue();
            guardNote.GetComponent<SpriteRenderer>().sprite = type switch
            {
                MoveType.High => highSprite,
                MoveType.Middle => middleSprite,
                MoveType.Low => lowSprite,
            };
        }

        private void ChangeSheet()
        {
            _sheetIndex++;
            if (_sheetIndex >= sheets.Count)
            {
                _isStart = false;
                return;
            }

            _noteIndex = 0;
            moveTime = sheets[_sheetIndex].sheetData.reachingTime;
        }
    }
}