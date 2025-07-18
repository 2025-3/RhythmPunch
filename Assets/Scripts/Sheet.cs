using System;
using UnityEngine;

public class Sheet : MonoBehaviour
{
    private SheetData _sheetData;
    public string sheetName;

    private string Path => $"Sheets/{sheetName}";
    
    private void Awake()
    {
        LoadSheet();
    }

    private void LoadSheet()
    {
        var jsonTextAsset = Resources.Load<TextAsset>(Path);
        if (jsonTextAsset)
        {
            var jsonString = jsonTextAsset.text;
            _sheetData = JsonUtility.FromJson<SheetData>(jsonString);
        }
    }

    public JudgementType Judge(int index, float time)
    {
        var needTime = _sheetData.notes[index].time;
        var timeDiff = Math.Abs(needTime - time);

        return timeDiff switch
        {
            _ when timeDiff > 0.5 => JudgementType.NoJudge,
            _ when timeDiff < 0.01 => JudgementType.Perfect,
            _ when timeDiff < 0.05 => JudgementType.Good,
            _ when timeDiff < 0.1 => JudgementType.Bad,
            _ => JudgementType.Miss
        };
    }
}
