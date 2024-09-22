#region

using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

#endregion

namespace Infrastructure.Factories.GameObjects
{
    public interface IGameObjectFactory
    {
        Task<GameObject> InstantiateAsync(
            string path,
            Vector3? position = null,
            Quaternion? rotation = null,
            Transform parent = null
        );

        Task<GameObject> InstantiateAsync(
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
        ) where T : class;

        Task<T> InstantiateAndGetComponent<T>(
            AssetReference path,
            Vector3? position = null,
            Quaternion? rotation = null,
            Transform parent = null
        ) where T : class;

        void Destroy(GameObject gameObject);
    }
}