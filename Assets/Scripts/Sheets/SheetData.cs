using System;

// 악보 정보 (Json용)
namespace Sheets
{
    [Serializable]
    public class SheetData
    {
        public int level;
        public double reachingTime;
        public NoteData[] notes;
    }
}
