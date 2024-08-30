using System;
using UnityEngine;

namespace Data.SceneContext
{
    [Serializable]
    public class MainMenuSceneContextData : BaseSceneContextData
    {
        [field: Header("UI")]
        [field: SerializeField] public Transform MainMenuScreenSpawnPoint { get; private set; }
        [field: SerializeField] public Transform StoreScreenSpawnPoint { get; private set; }
    }
}