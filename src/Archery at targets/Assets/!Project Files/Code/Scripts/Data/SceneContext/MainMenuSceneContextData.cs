#region

using System;
using UnityEngine;

#endregion

namespace Data.SceneContext
{
    [Serializable]
    public class MainMenuSceneContextData : BaseSceneContextData
    {
        [field: Header("UI")]
        [field: SerializeField] public Transform MainMenuScreenSpawnPoint { get; private set; }
        [field: SerializeField] public Transform LevelsScreenSpawnPoint { get; private set; }
    }
}