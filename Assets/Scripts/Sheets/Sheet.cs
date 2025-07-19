using System;
using System.IO;
using UnityEngine;

// 악보 래핑 클래스
namespace Sheets
{
    public class Sheet : MonoBehaviour
    {
        public SheetData sheetData; // 악보 데이터
        public string sheetName; // 악보 파일 이름; 오브젝트에서 지정 필요
    
        // Awake에서 악보 로드
        private void Awake()
        {
            LoadSheet();
        }

        // 악보 로드 메소드
        private void LoadSheet()
        {
            var path = Path.Combine(Application.streamingAssetsPath, $"Sheets/{sheetName}.json");
            var json = File.ReadAllText(path);
            sheetData = JsonUtility.FromJson<SheetData>(json);
        }

        // 판정; 숫자는 임시임
        public JudgementType Judge(int index, float time)
        {
            var needTime = sheetData.notes[index].time;
            var timeDiff = Math.Abs(needTime - time);

            return timeDiff switch
            {
                _ when timeDiff > 0.5 => JudgementType.NoJudge,
                _ when timeDiff < 0.1 => JudgementType.Perfect,
                _ when timeDiff < 0.5 => JudgementType.Good,
                _ when timeDiff < 1 => JudgementType.Bad,
                _ => JudgementType.Miss
            };
        }
    }
}
