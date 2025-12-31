using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Networking;

// 악보 래핑 클래스
namespace Sheets
{
    public class Sheet : MonoBehaviour
    {
        public SheetData sheetData; // 악보 데이터
        public string sheetName; // 악보 파일 이름; 오브젝트에서 지정 필요

        private string SheetPath
        {
            get
            {
#if UNITY_EDITOR || UNITY_STANDALONE
                return Path.Combine(Application.streamingAssetsPath, $"Sheets/{sheetName}.json");
#elif UNITY_WEBGL
                return Path.Combine(Application.streamingAssetsPath, $"Sheets/{sheetName}.json?t={System.DateTime.Now.Ticks}");
#else
                Assert.IsTrue(false, "지원되지 않는 플랫폼입니다.");
#endif
            }
        }

        private void Awake()
        {
        }

        private void Start()
        {
        }

        public IEnumerator LoadSheet()
        {
            string json = null;
            
#if UNITY_EDITOR || UNITY_STANDALONE
            Assert.IsTrue(File.Exists(SheetPath), "경로에 파일이 존재하지 않습니다.");
            
            json = File.ReadAllText(SheetPath);
#elif UNITY_WEBGL
            using var req = UnityWebRequest.Get(SheetPath);
            yield return req.SendWebRequest();
            
            Assert.IsTrue(req.result == UnityWebRequest.Result.Success, "WebRequest가 실패했습니다.");
            
            json = req.downloadHandler.text;
#else
            Assert.IsTrue(false, "지원되지 않는 플랫폼입니다.");            
#endif
            Assert.IsFalse(string.IsNullOrEmpty(json), "json을 제대로 가져오지 못했습니다.");
            
            sheetData = JsonUtility.FromJson<SheetData>(json);
            
            Assert.IsNotNull(sheetData, "json 파싱에 실패했습니다.");
            
            yield break;
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
