using System.Collections.Generic;
using System.Linq;
using Sheets;
using UnityEngine;

namespace ObjectControls
{
    public class ComboBarControl : MonoBehaviour
    {
        private List<MoveType> _comboList;
        
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
                    _comboList.Clear();
                    break;
                case NoteType.Attack:
                case NoteType.Guard when GameManager.Instance.NowModeCount == 0:
                    _comboList = GameManager.Instance.CommandList.ToList();
                    break;
            }

            DrawComboBar();
        }

        private void DrawComboBar()
        {
            for (int i = 0; i < _comboList.Count; i++)
            {
                // Draw 
            }
        }
    }
}
