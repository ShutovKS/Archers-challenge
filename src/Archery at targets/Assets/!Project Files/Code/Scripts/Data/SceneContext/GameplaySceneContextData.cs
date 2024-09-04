#region

using Features.PositionsContainer;
using UnityEngine;

#endregion

namespace Data.SceneContext
{
<<<<<<<< HEAD:src/Archery at targets/Assets/!Project Files/Code/Scripts/Data/SceneContext/InfiniteSceneContextData.cs
    public class InfiniteSceneContextData : BaseSceneContextData
========
    public class GameplaySceneContextData : BaseSceneContextData
>>>>>>>> dev:src/Archery at targets/Assets/!Project Files/Code/Scripts/Data/SceneContext/GameplaySceneContextData.cs
    {
        [field: Header("Spawn Points")]
        [field: SerializeField] public Transform BowSpawnPoint { get; private set; }
        [field: SerializeField] public Transform InfoScreenSpawnPoint { get; private set; }
        
        [field: Header("Position Containers")]
        [field: SerializeField] public PositionsContainer PositionsContainer { get; private set; }
    }
}