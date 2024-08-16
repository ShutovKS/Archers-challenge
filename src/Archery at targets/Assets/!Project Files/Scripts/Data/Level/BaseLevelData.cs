using UnityEngine;

namespace Data.Level
{
    public abstract class BaseLevelData : ScriptableObject
    {
        [field: SerializeField] public string Key { get; private set; }
        [field: SerializeField] public SpawnPoint PlayerSpawnPoint { get; private set; }
    }
}