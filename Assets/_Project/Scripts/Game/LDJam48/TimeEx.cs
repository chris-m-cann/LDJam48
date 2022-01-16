using System.Collections.Generic;
using UnityEngine;
using Util;
using Util.Var.Observe;

namespace LDJam48
{
    public class TimeEx : MonoBehaviour
    {
        [SerializeField] private ObservableBoolVariable isPaused;
        [Range(0, 1)] [SerializeField] private float timeScale = 1;
        
        private Dictionary<int, float> _scalesInEffect = new Dictionary<int, float>();
        private int _nextId = 0;
        private int _maxIds = 100;
        private int _pauseId;

        private void OnValidate()
        {
            Time.timeScale = timeScale;
        }
        
        private void OnEnable()
        {
            if (isPaused != null)
            {
                isPaused.OnValueChanged += HandlePause;

                isPaused.Reset();
            }

        }

        private void OnDisable()
        {
            if (isPaused != null)
            {
                isPaused.OnValueChanged -= HandlePause;
            }

            Time.timeScale = 1f;
        }

        private void HandlePause(bool pause)
        {
            if (pause)
            {
                _pauseId = PushTimeScale(0f);
            }
            else
            {
                PopTimeScale(_pauseId);
            }
        }
        
        public int SetTimeScaleForDuration(float scale, float duration)
        {
            int id = PushTimeScale(scale);

            this.ExecuteAfterUnscaled(duration, () => PopTimeScale(id));

            return id;
        }
        
        
        public void SetTimeScaleForDuration(Vector2 request) => SetTimeScaleForDuration(request.x, request.y);

        public int PushTimeScale(float scale)
        {
            int id = _nextId;
            _nextId++;
            if (_nextId > _maxIds)
            {
                _nextId = 0;
            }
         
            _scalesInEffect.Add(id, scale);
            
            if (scale < Time.timeScale)
            {
                Time.timeScale = scale;
            }

            return id;
        }

        public void PopTimeScale(int id)
        {
            _scalesInEffect.Remove(id);

            float minScale = timeScale;
            foreach (var it in _scalesInEffect)
            {
                if (it.Value < minScale)
                {
                    minScale = it.Value;
                }
            }

            Time.timeScale = minScale;
        }
    }
}