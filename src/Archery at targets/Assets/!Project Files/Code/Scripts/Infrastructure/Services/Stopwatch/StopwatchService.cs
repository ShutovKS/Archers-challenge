using System;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace Infrastructure.Services.Stopwatch
{
    [UsedImplicitly]
    public class StopwatchService : IStopwatchService, ITickable
    {
        public float CurrentTime { get; private set; }

        public event Action<float> OnTick;

        private bool _isRunning;

        public void Start()
        {
            _isRunning = true;
            
            CurrentTime = 0;
        }

        public void Stop()
        {
            _isRunning = false;
        }

        public void Tick()
        {
            if (!_isRunning)
            {
                return;
            }

            CurrentTime += Time.deltaTime;
            OnTick?.Invoke(CurrentTime);
        }
    }
}