using UnityEngine;

namespace Data.Configurations.Level
{
    public enum GameplayModeType
    {
        None = 0,

        // Gameplay
        InfiniteVR = 1,
        InfiniteMR = 2,

        DestroyingAllTargets = 11,
        DestroyingAllTargetsInTime = 12,
    }

    public abstract class GameplayModeData
    {
        public abstract GameplayModeType ModeType { get; }
    }

    public class InfiniteVRGameplay : GameplayModeData
    {
        public override GameplayModeType ModeType => GameplayModeType.InfiniteVR;
    }

    public class InfiniteMRGameplay : GameplayModeData
    {
        public override GameplayModeType ModeType => GameplayModeType.InfiniteMR;
    }

    public class DestroyingNTargetsGameplay : GameplayModeData
    {
        public override GameplayModeType ModeType => GameplayModeType.DestroyingAllTargets;

        [field: SerializeField] public int TargetCount { get; private set; }
    }
}