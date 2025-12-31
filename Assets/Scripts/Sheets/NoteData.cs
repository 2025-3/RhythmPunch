using System;

// 노트 정보 (Json용);
namespace Sheets
{
    [Serializable]
    public class NoteData
    {
        public double time;
        public NoteType noteType;
    }
}
