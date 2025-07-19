using System;

// 악보 정보 (Json용)
namespace Sheets
{
    [Serializable]
    public class SheetData
    {
        public NoteData[] notes;
    }
}
