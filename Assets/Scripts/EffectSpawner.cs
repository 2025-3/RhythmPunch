using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sheets;
public class EffectSpawner : MonoBehaviour
{
    public static EffectSpawner Instance;

    public List<GameObject> effects;

    public GameObject highSpawnPoint;
    public GameObject middleSpawnPoint;
    public GameObject lowSpawnPoint;


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
        
    }
    public void SpawnEffect(MoveType Type)
    {
        if (Type == MoveType.High) 
        {
            Instantiate(effects[0], highSpawnPoint.transform);


        }
        else if (Type == MoveType.Middle) 
        {
            Instantiate(effects[1], middleSpawnPoint.transform);

        }
        else if (Type == MoveType.Low)
        {
            Instantiate(effects[2], lowSpawnPoint.transform);

        }


    }
}
