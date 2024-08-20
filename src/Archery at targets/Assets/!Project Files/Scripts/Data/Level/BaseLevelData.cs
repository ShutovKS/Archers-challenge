using UnityEngine;

namespace Data.Level
{
    public abstract class BaseLevelData : ScriptableObject
    {
        public abstract string Key { get; protected set; }
        [field: SerializeField] public SpawnPoint PlayerSpawnPoint { get; private set; }
        [field: SerializeField] public string LocationPrefabPath { get; private set; }
    }
}