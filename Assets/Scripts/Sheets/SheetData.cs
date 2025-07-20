using System;

// 악보 정보 (Json용)
namespace Sheets
{
    [Serializable]
    public class SheetData
    {
        public int level;
        public float reachingTime;
        public NoteData[] notes;
    }
}
