using System;
using UnityEngine;

namespace Data.Level
{
    [Serializable]
    public class SpawnPoint
    {
        [field: SerializeField] public Vector3 Position { get; protected set; }
        [field: SerializeField] public Vector3 Rotation { get; protected set; }
    }
}