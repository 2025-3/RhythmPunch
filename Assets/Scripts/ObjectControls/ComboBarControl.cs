using System.Collections.Generic;
using System.Linq;
using Sheets;
using UnityEngine;

namespace ObjectControls
{
    public class ComboBarControl : MonoBehaviour
    {
        public GameObject high;
        public GameObject middle;
        public GameObject low;
        public float interval = 1.0f;
        
        private List<MoveType> _comboList = new();
        private List<GameObject> _goList = new();
        
        // Start is called before the first frame update
        private void Start()
        {
            GameManager.Instance.onNoteDestroyed.AddListener((_, _) => { UpdateComboBar(); });
        }

        private void UpdateComboBar()
        {
            switch (GameManager.Instance.Mode)
            {
                case NoteType.Attack when GameManager.Instance.NowModeCount == 0:
                case NoteType.Guard when GameManager.Instance.NowModeCount == GameManager.Instance.NowModeLength - 1:
                    _comboList.Clear();
                    break;
                case NoteType.Attack:
                    _comboList = GameManager.Instance.CommandList.ToList();
                    break;
            }

            DrawComboBar();
        }

        private void DrawComboBar()
        {
            foreach (var go in _goList)
                Destroy(go);
            _goList.Clear();
                
            for (int i = 0; i < _comboList.Count; i++)
            {
                var go = _comboList[i] switch
                {
                    MoveType.High => Instantiate(high),
                    MoveType.Middle => Instantiate(middle),
                    MoveType.Low => Instantiate(low),
                };
                go.transform.position = transform.position + new Vector3(0, -interval + interval * i, 0);
                _goList.Add(go);
            }
        }
    }
}
