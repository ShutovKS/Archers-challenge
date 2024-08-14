using UnityEngine;

namespace Data.Level
{
    public abstract class BaseLevelData : ScriptableObject
    {
        [field: SerializeField] public CreatedObjectData PlayerCreatedObjectData { get; private set; }
    }
}