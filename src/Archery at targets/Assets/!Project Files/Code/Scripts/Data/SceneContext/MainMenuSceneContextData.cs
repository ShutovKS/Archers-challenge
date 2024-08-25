using System;
using UnityEngine;

namespace Data.SceneContext
{
    [Serializable]
    public class MainMenuSceneContextData : BaseSceneContextData
    {
        [field: Header("Main Menu")]
        [field: SerializeField] public Transform MainMenuScreenSpawnPoint { get; private set; }
    }
}