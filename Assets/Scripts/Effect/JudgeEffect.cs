using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JudgeEffect : MonoBehaviour
{
    public static float lifetime = 1f;
    public static float speed = 5f;
    public static float distance = 1.5f;

    private Vector3 _dest;

    private void Start()
    {
        //Destroy(gameObject,1f);
        _dest = transform.position + Vector3.up * distance * transform.lossyScale.x;
    }
    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, _dest, speed * Time.deltaTime);
    }
}
