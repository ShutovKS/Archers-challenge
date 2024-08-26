using System.IO;
using UnityEngine;

namespace Infrastructure.Services.DataStorage
{
    public class DataStorageLocalService : IDataStorageService
    {
        public void Save<T>(string key, T data)
        {
            var json = JsonUtility.ToJson(data);
            File.WriteAllText(Application.persistentDataPath + "/" + key + ".json", json);
        }

        public T Load<T>(string key)
        {
            var path = Application.persistentDataPath + "/" + key + ".json";
            if (!File.Exists(path))
            {
                return default;
            }

            var json = File.ReadAllText(path);
            return JsonUtility.FromJson<T>(json);
        }

        public bool HasKey(string key)
        {
            return File.Exists(Application.persistentDataPath + "/" + key + ".json");
        }
    }
}