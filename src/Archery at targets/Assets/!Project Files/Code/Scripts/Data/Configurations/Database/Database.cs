using System;
using System.Collections.Generic;
using System.Linq;
using Data.Constants.Paths;
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
            RemovedDuplicateItems();
        }

        public void OnValidate(T item)
        {
            if (Items.Contains(item)) return;
            Items.Add(item);
        }

        private void RemovedNullItems() => Items = Items?.Where(item => item != null).ToList() ?? new List<T>();
        private void RemovedDuplicateItems() => Items = Items?.Distinct().ToList() ?? new List<T>();

        private static Database<T> LoadOrCreateInstance()
        {
            var instance = Resources.Load<Database<T>>($"{ResourcesPaths.DATABASES}{typeof(T).Name}base");
            return instance ? instance : CreateAndSaveInstance();
        }

        private static Database<T> CreateAndSaveInstance()
        {
#if UNITY_EDITOR
            Debug.LogWarning($"Database {typeof(T).Name} not found in Resources/Data/Databases, creating new one");

            var instance = ScriptableObject.CreateInstance(typeof(T).Name + "base") as Database<T>;
            if (instance == null) throw new Exception($"Failed to create database for {typeof(T).Name}");

            if (!System.IO.Directory.Exists($"Assets/Resources/{ResourcesPaths.DATABASES}"))
            {
                Debug.LogWarning($"Creating directory {ResourcesPaths.DATABASES}");
                System.IO.Directory.CreateDirectory($"Assets/Resources/{ResourcesPaths.DATABASES}");
            }

            UnityEditor.AssetDatabase.CreateAsset(instance, $"Assets/Resources/{ResourcesPaths.DATABASES}{typeof(T).Name}base.asset");
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
            return instance;
#else
        throw new Exception($"Database {typeof(T).Name} not found in Resources/Data/Databases");
#endif
        }
    }
}