using System.Threading.Tasks;
using Infrastructure.Services.AssetsAddressables;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Infrastructure.Factories.GameObjects
{
    [UsedImplicitly]
    public class GameObjectFactory : IGameObjectFactory
    {
        private readonly DiContainer _container;
        private readonly IAssetsAddressablesProvider _assetsAddressablesProvider;

        public GameObjectFactory(DiContainer container, IAssetsAddressablesProvider assetsAddressablesProvider)
        {
            _container = container;
            _assetsAddressablesProvider = assetsAddressablesProvider;
        }

        public async Task<GameObject> Instantiate(string path, Vector3? position = null, Quaternion? rotation = null,
            Transform parent = null)
        {
            position ??= Vector3.zero;
            rotation ??= Quaternion.identity;

            var prefab = await _assetsAddressablesProvider.GetAsset<GameObject>(path);
            var instance = _container.InstantiatePrefab(prefab, position.Value, rotation.Value, parent);
            return instance;
        }

        public async Task<GameObject> Instantiate(AssetReference path, Vector3? position = null, Quaternion? rotation = null,
            Transform parent = null)
        {
            position ??= Vector3.zero;
            rotation ??= Quaternion.identity;

            var prefab = await _assetsAddressablesProvider.GetAsset<GameObject>(path);
            var instance = _container.InstantiatePrefab(prefab, position.Value, rotation.Value, parent);
            return instance;
        }

        public async Task<T> InstantiateAndGetComponent<T>(string path, Vector3? position = null, Quaternion? rotation = null,
            Transform parent = null) where T : Component
        {
            var instance = await Instantiate(path, position, rotation, parent);
            return instance.GetComponent<T>();
        }

        public async Task<T> InstantiateAndGetComponent<T>(AssetReference path, Vector3? position = null, Quaternion? rotation = null,
            Transform parent = null) where T : Component
        {
            var instance = await Instantiate(path, position, rotation, parent);
            return instance.GetComponent<T>();
        }
        
        public void Destroy(GameObject gameObject)
        {
            Object.Destroy(gameObject);
        }
    }
}