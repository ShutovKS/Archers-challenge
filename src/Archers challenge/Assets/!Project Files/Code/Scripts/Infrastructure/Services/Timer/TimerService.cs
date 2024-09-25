#region

using System;
using UnityEngine;
using Zenject;

#endregion

namespace Infrastructure.Services.Timer
{
    public class TimerService : ITimerService, ITickable
    {
        public float RemainingTime { get; private set; }

        public event Action<float> OnTick;
        public event Action OnFinished;

        private bool _isRunning;

        public void Start(float duration)
        {
            if (_isRunning)
            {
                throw new Exception("Failed attempt to start TimerService, it is already running");
            }

            RemainingTime = duration;
            _isRunning = true;
        }

        public void Stop()
        {
            if (!_isRunning)
            {
                throw new Exception("Unsuccessful attempt to stop TimerService, it is not running");
            }

            _isRunning = false;
        }

        public void Tick()
        {
            if (!_isRunning)
            {
                return;
            }

            RemainingTime -= Time.deltaTime;
            OnTick?.Invoke(RemainingTime);

            if (RemainingTime > 0)
            {
                return;
            }

            _isRunning = false;
            OnFinished?.Invoke();
        }
    }
}