using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Data.Configurations.Database
{
    public abstract class Database<T> : ScriptableObject where T : ScriptableObject
    {
        private static Database<T> _instance;
        public static Database<T> Instance => _instance ??= LoadOrCreateInstance();

        [field: SerializeField] public List<T> Items { get; private set; } = new();

        public void OnValidate()
        {
            RemovedNullItems();
        }

        public void OnValidate(T item)
        {
            if (Items.Contains(item)) return;
            Items.Add(item);
        }
        
        private void RemovedNullItems()
        {
            Items = Items?.Where(item => item != null).ToList() ?? new List<T>();
        }

        private static Database<T> LoadOrCreateInstance()
        {
            var instance = Resources.Load<Database<T>>($"Data/Databases/{typeof(T).Name}base");
            return instance ? instance : CreateAndSaveInstance();
        }

        private static Database<T> CreateAndSaveInstance()
        {
#if UNITY_EDITOR
            var instance = ScriptableObject.CreateInstance(typeof(T).Name + "base") as Database<T>;
            if (instance == null) throw new Exception($"Failed to create database for {typeof(T).Name}");
            
            UnityEditor.AssetDatabase.CreateAsset(instance, $"Assets/!Project Files/Resources/Data/Databases/{typeof(T).Name}base.asset");
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
            return instance;
#else
        throw new Exception($"Database {typeof(T).Name} not found in Resources/Data/Databases");
#endif
        }
    }
}