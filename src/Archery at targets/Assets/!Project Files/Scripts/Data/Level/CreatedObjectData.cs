using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Data.Level
{
    [Serializable]
    public class CreatedObjectData
    {
        [field: SerializeField] public AssetReference AssetReference { get; protected set; }
        [field: SerializeField] public Vector3 Position { get; protected set; }
        [field: SerializeField] public Vector3 Rotation { get; protected set; }
    }
}