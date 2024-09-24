#region

using Features.TargetsInLevelManager;
using UnityEngine;

#endregion

namespace Data.Contexts.Scene
{
    public class GameplaySceneContextData : BaseSceneContextData
    {
        [field: SerializeField] public Transform PlayerSpawnPoint { get; private set; }
        [field: SerializeField] public Transform BowSpawnPoint { get; private set; }
        [field: SerializeField] public Transform InfoScreenSpawnPoint { get; private set; }

        [field: Space, SerializeField] public TargetsInLevelManager TargetsInLevelManager { get; private set; }

        [field: Space, SerializeField] public float BowForce { get; private set; } = 15f;
    }
}