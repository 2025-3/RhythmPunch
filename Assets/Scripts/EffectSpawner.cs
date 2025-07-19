using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sheets;
public class EffectSpawner : MonoBehaviour
{
    public static EffectSpawner Instance;

    public GameObject missEffect;
    public GameObject badEffect;
    public GameObject goodEffect;
    public GameObject perfectEffect;

    public GameObject spawnPoint;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        if (spawnPoint == null) spawnPoint = new GameObject();
        
    }
    public void SpawnEffect(JudgementType judgementType)
    {
        GameObject v;

        if (judgementType == JudgementType.Miss)
            v = Instantiate(missEffect, spawnPoint.transform);
        else if (judgementType == JudgementType.Bad)
            v = Instantiate(badEffect, spawnPoint.transform);
        else if (judgementType == JudgementType.Good)
            v = Instantiate(goodEffect, spawnPoint.transform);
        else 
            v = Instantiate(perfectEffect, spawnPoint.transform);

        Destroy(v, 2);
    }
}
