using System;

namespace Infrastructure.Services.Stopwatch
{
    public interface IStopwatchService
    {
        float CurrentTime { get; }

        event Action<float> OnTick;

        void Start();
        void Stop();
    }
}