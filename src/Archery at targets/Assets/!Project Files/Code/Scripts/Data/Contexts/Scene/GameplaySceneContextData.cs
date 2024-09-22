#region

using Features.PositionsContainer;
using UnityEngine;

#endregion

namespace Data.Contexts.Scene
{
    public class GameplaySceneContextData : BaseSceneContextData
    {
        [field: Header("Spawn Points")]
        [field: SerializeField]
        public Transform PlayerSpawnPoint { get; private set; }

        [field: SerializeField] public Transform BowSpawnPoint { get; private set; }
        [field: SerializeField] public Transform InfoScreenSpawnPoint { get; private set; }

        [field: Header("Position Containers")]
        [field: SerializeField]
        public PositionsContainer PositionsContainer { get; private set; }
    }
}