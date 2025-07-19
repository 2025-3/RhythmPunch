using System;
using UnityEngine;

namespace ObjectControls
{
    public class NoteControl : MonoBehaviour
    {
        public Vector2 startPos;
        public Vector2 endPos;
        public float moveTime;

        private Vector2 Direction => (endPos - startPos).normalized;
        private float Speed => (endPos - startPos).magnitude / moveTime;
        
        private void Awake()
        {
            
        }

        private void Start()
        {
            transform.position = startPos;
        }

        private void Update()
        {
            transform.position += Speed * Time.deltaTime * (Vector3)Direction;
        }
    }
}