using System;
using UnityEngine;
using Zenject;

namespace Infrastructure.Services.Stopwatch
{
    public class StopwatchService : IStopwatchService, ITickable
    {
        public float CurrentTime { get; private set; }

        public event Action<float> OnTick;

        private bool _isRunning;

        public void Start()
        {
            if (_isRunning)
            {
                throw new Exception("Failed attempt to start StopwatchService, it is already running");
            }

            _isRunning = true;
        }

        public void Stop()
        {
            if (!_isRunning)
            {
                throw new Exception("Unsuccessful attempt to stop StopwatchService, it is not running");
            }

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