using System.Threading.Tasks;
using Infrastructure.Services.AssetsAddressables;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Infrastructure.Factories.GameObjects
{
    public class GameObjectFactory : IGameObjectFactory
    {
        private readonly DiContainer _container;
        private readonly IAssetsAddressablesProvider _assetsProvider;

        public GameObjectFactory(DiContainer container, IAssetsAddressablesProvider assetsProvider)
        {
            _container = container;
            _assetsProvider = assetsProvider;
        }

        public async Task<GameObject> InstantiateAsync(string path, Vector3? position = null,
            Quaternion? rotation = null, Transform parent = null)
        {
            return InstantiateAsync(await _assetsProvider.GetAsset<GameObject>(path), position, rotation, parent);
        }

        public async Task<GameObject> InstantiateAsync(AssetReference path, Vector3? position = null,
            Quaternion? rotation = null, Transform parent = null)
        {
            return InstantiateAsync(await _assetsProvider.GetAsset<GameObject>(path), position, rotation, parent);
        }

        public async Task<T> InstantiateAndGetComponent<T>(string path, Vector3? position = null,
            Quaternion? rotation = null, Transform parent = null) where T : Component
        {
            return (await InstantiateAsync(path, position, rotation, parent)).GetComponent<T>();
        }

        public async Task<T> InstantiateAndGetComponent<T>(AssetReference path, Vector3? position = null,
            Quaternion? rotation = null, Transform parent = null) where T : Component
        {
            return (await InstantiateAsync(path, position, rotation, parent)).GetComponent<T>();
        }

        public void Destroy(GameObject gameObject)
        {
            Object.Destroy(gameObject);
        }

        private GameObject InstantiateAsync(GameObject prefab, Vector3? position = null, Quaternion? rotation = null,
            Transform parent = null)
        {
            return _container.InstantiatePrefab(prefab, position ?? Vector3.zero, rotation ?? Quaternion.identity,
                parent);
        }
    }
}