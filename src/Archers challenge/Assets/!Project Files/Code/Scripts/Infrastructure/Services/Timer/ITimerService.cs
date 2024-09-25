#region

using System;

#endregion

namespace Infrastructure.Services.Timer
{
    public interface ITimerService
    {
        float RemainingTime { get; }

        event Action<float> OnTick;
        event Action OnFinished;

        void Start(float durationSec);
        void Stop();
    }
}