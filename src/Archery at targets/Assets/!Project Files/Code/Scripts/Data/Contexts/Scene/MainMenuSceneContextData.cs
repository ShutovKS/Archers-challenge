#region

using System;
using UnityEngine;

#endregion

namespace Data.Contexts.Scene
{
    [Serializable]
    public class MainMenuSceneContextData : BaseSceneContextData
    {
        [field: Header("Spawn Points")]
        [field: SerializeField] public Transform PlayerSpawnPoint { get; private set; }
        [field: SerializeField] public Transform MainMenuScreenSpawnPoint { get; private set; }
        [field: SerializeField] public Transform LevelsScreenSpawnPoint { get; private set; }
    }
}