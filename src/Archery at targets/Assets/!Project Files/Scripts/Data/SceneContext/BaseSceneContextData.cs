using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Data.SceneContext
{
    public abstract class BaseSceneContextData
    {
        [field: Header("Player")]
        [field: SerializeField] public Transform PlayerSpawnPoint { get; private set; }
    }

    [Serializable]
    public class MainMenuSceneContextData : BaseSceneContextData
    {
        [field: Header("Main Menu")]
        [field: SerializeField] public Transform MainMenuScreenSpawnPoint { get; private set; }
    }
}