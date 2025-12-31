using System;
using UnityEngine;

namespace ObjectControls
{
    public class NoteControl : MonoBehaviour
    {
        public Vector2 startPos;
        public Vector2 endPos;
        public double moveTime;

        private Vector2 Direction => (endPos - startPos).normalized;
        private double Speed => (endPos - startPos).magnitude / moveTime;
        public double spawnTime;
        private double _smoothTime;
        private const double SmoothFactor = 0.5;
        
        private bool _isMove = false;

        private void Awake()
        {
        }

        private void Start()
        {
            transform.position = startPos;
        }

        private void Update()
        {
            if (!_isMove)
                return;

            var realTime = AudioSettings.dspTime;
            var error = realTime - _smoothTime;
            _smoothTime += Time.deltaTime * (1.0 + error * SmoothFactor);

            transform.position = startPos + (float)(Speed * (_smoothTime - spawnTime)) * Direction;
        }

        public void StartMove()
        {
            _isMove = true;
            _smoothTime = spawnTime;
        }
    }
}