using UnityEngine;

namespace Data.Gameplay
{
    public enum GameplayMode
    {
        None = 0,

        // Gameplay
        Infinite = 1,

        DestroyingAllTargets = 2,
        DestroyingAllTargetsInTime = 3,
    }

    public abstract class GameplayModeData
    {
        public abstract GameplayMode Mode { get; }
    }

    public class InfiniteGameplay : GameplayModeData
    {
        public override GameplayMode Mode => GameplayMode.Infinite;
    }

    public class DestroyingAllTargetsGameplay : GameplayModeData
    {
        public override GameplayMode Mode => GameplayMode.DestroyingAllTargets;

        [field: SerializeField] public int TargetCount { get; private set; }
    }

    public class DestroyingAllTargetsInTimeGameplay : GameplayModeData
    {
        public override GameplayMode Mode => GameplayMode.DestroyingAllTargetsInTime;

        [field: SerializeField] public int TargetCount { get; private set; }
        [field: SerializeField] public float TimeLimit { get; private set; }
    }
}