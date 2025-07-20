using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sheets;
public class EffectSpawner : MonoBehaviour
{
    public static EffectSpawner Instance;

    public List<GameObject> effects;

    [Header("Player Hit")]
    public GameObject p_highSpawnPoint;
    public GameObject p_middleSpawnPoint;
    public GameObject p_lowSpawnPoint;

    [Header("Enemy Hit")]
    public GameObject e_highSpawnPoint;
    public GameObject e_middleSpawnPoint;
    public GameObject e_lowSpawnPoint;

    [Header("Note Hit")]
    public GameObject notePoint;

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
        GameManager.Instance.onCounterAttacked.AddListener(HitEnemyEffect);

    }
    public void HitPlayerEffect(MoveType Type)
    {
        if (Type == MoveType.High) 
        {
            Instantiate(effects[0], p_highSpawnPoint.transform);


        }
        else if (Type == MoveType.Middle) 
        {
            Instantiate(effects[1], p_middleSpawnPoint.transform);

        }
        else if (Type == MoveType.Low)
        {
            Instantiate(effects[2], p_lowSpawnPoint.transform);

        }

    }
    public void DefendPlayer()
    {
        Instantiate(effects[3], p_middleSpawnPoint.transform);
    }
    public void HitEnemyEffect(MoveType Type)
    {
        if (Type == MoveType.High)
        {
            Instantiate(effects[0], e_highSpawnPoint.transform);
        }
        else if (Type == MoveType.Middle)
        {
            Instantiate(effects[1], e_middleSpawnPoint.transform);

        }
        else if (Type == MoveType.Low)
        {
            Instantiate(effects[2], e_lowSpawnPoint.transform);

        }


    }
    public void HitEnemyEffect()
    {
        MoveType Type = InputProcessor.currentInput;
        if (Type == MoveType.High)
        {
            Instantiate(effects[0], e_highSpawnPoint.transform);
        }
        else if (Type == MoveType.Middle)
        {
            Instantiate(effects[1], e_middleSpawnPoint.transform);

        }
        else if (Type == MoveType.Low)
        {
            Instantiate(effects[2], e_lowSpawnPoint.transform);

        }


    }
}
