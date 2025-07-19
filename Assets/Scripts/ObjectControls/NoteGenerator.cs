using System.Collections.Generic;
using Sheets;
using UnityEngine;

namespace ObjectControls
{
    public class NoteGenerator : MonoBehaviour
    {
        public NoteControl note;

        public Vector2 startPos;
        public Vector2 endPos;
        public float moveTime;

        private readonly Queue<NoteControl> _noteQueue = new();

        private bool _isStart = false;
        private int _noteIndex = 0;
        private float _startTime;
        private float CurrentTime => Time.time - _startTime;
        
        private void Awake()
        {
            
        }

        private void Start()
        {
            GameManager.Instance.onStartGame.AddListener(StartGenerate);
            GameManager.Instance.onNoteDestroyed.AddListener((_, reason) => { DestroyNote(reason); });
        }

        private void Update()
        {
            if (_isStart)
            {
                if (_noteIndex >= GameManager.Instance.sheet.sheetData.notes.Length)
                {
                    _isStart = false;
                }
                else if (CurrentTime >= GameManager.Instance.sheet.sheetData.notes[_noteIndex].time - moveTime)
                {
                    GenerateNote();
                    _noteIndex++;
                }
            }
        }
        
        public void StartGenerate()
        {
            _isStart = true;
            _startTime = Time.time;
        }
        
        public void GenerateNote()
        {
            var newNote = Instantiate(note, startPos, Quaternion.identity);
            newNote.startPos = startPos;
            newNote.endPos = endPos;
            newNote.moveTime = moveTime;
            
            _noteQueue.Enqueue(newNote);
        }

        public void DestroyNote(JudgementType reason)
        {
            var oldNote = _noteQueue.Dequeue();

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
            
            
            Destroy(oldNote.gameObject);
        }
    }
}