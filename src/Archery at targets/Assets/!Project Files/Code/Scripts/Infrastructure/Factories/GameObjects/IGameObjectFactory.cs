using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Infrastructure.Factories.GameObjects
{
    public interface IGameObjectFactory
    {
        Task<GameObject> Instantiate(
            string path,
            Vector3? position = null,
            Quaternion? rotation = null,
            Transform parent = null
        );

        Task<GameObject> Instantiate(
            AssetReference path,
            Vector3? position = null,
            Quaternion? rotation = null,
            Transform parent = null
        );

        Task<T> InstantiateAndGetComponent<T>(
            string path,
            Vector3? position = null,
            Quaternion? rotation = null,
            Transform parent = null
        ) where T : Component;

        Task<T> InstantiateAndGetComponent<T>(
            AssetReference path,
            Vector3? position = null,
            Quaternion? rotation = null,
            Transform parent = null
        ) where T : Component;
        
        void Destroy(GameObject gameObject);
    }
}