#region

using UnityEngine;

#endregion

namespace Data.Configurations.Database
{
    public abstract class Database<T> : ScriptableObject where T : ScriptableObject
    {
        [field: SerializeField] public T[] Items { get; private set; }
    }
}