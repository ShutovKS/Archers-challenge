using System;

namespace Features.TargetsInLevelManager
{
    public interface ITargetsInLevelManager
    {
        event Action OnTargetHit;

        void PrepareTargets();
        void StartTargets();
        void StopTargets();
    }
}