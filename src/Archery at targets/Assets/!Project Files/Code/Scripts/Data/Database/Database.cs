using UnityEngine;

namespace Data.Database
{
    public abstract class Database<T> : ScriptableObject where T : ScriptableObject
    {
        [field: SerializeField] public T[] Items { get; private set; }
    }
}