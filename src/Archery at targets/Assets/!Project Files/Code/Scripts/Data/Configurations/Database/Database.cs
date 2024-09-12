#region

using System;
using UnityEngine;

#endregion

namespace Data.Configurations.Database
{
    public abstract class Database<T> : ScriptableObject where T : ScriptableObject
    {
        [field: SerializeField] public T[] Items { get; private set; }

        public void OnValidate()
        {
            if (Items is { Length: > 1 })
            {
                Items = Array.FindAll(Items, item => item != null);
            }
            
            if (Items == null || Items.Length == 0)
            {
                Items = new T[1];
            }
        }
    }
}