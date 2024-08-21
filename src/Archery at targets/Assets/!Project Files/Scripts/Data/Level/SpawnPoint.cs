using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Data.Level
{
    [Serializable]
    public class SpawnPoint
    {
        [field: SerializeField] public Vector3 Position { get; protected set; }
        [field: SerializeField] public Vector3 Rotation { get; protected set; }
    }

    [Serializable]
    public class SpawnObject : SpawnPoint
    {
        [field: SerializeField] public AssetReference Object { get; protected set; }
    }
}