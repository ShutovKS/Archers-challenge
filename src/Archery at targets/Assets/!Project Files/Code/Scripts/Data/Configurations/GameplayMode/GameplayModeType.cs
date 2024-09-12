using UnityEngine;

namespace Data.Configurations.GameplayMode
{
    public enum GameplayModeType
    {
        None = 0,

        // Gameplay
        Infinite = 1,

        DestroyingAllTargets = 2,
        DestroyingAllTargetsInTime = 3,
    }

    public abstract class GameplayModeData
    {
        public abstract GameplayModeType ModeType { get; }
    }

    public class InfiniteGameplay : GameplayModeData
    {
        public override GameplayModeType ModeType => GameplayModeType.Infinite;
    }

    public class DestroyingAllTargetsGameplay : GameplayModeData
    {
        public override GameplayModeType ModeType => GameplayModeType.DestroyingAllTargets;

        [field: SerializeField] public int TargetCount { get; private set; }
    }

    public class DestroyingAllTargetsInTimeGameplay : GameplayModeData
    {
        public override GameplayModeType ModeType => GameplayModeType.DestroyingAllTargetsInTime;

        [field: SerializeField] public int TargetCount { get; private set; }
        [field: SerializeField] public float TimeLimit { get; private set; }
    }
}