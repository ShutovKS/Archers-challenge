using System.Threading.Tasks;
using Infrastructure.Services.AssetsAddressables;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Infrastructure.Factories.GameObjects
{
    public class GameObjectFactory : IGameObjectFactory
    {
        private readonly DiContainer _container;
        private readonly IAssetsAddressablesProvider _assetsAddressablesProvider;

        public GameObjectFactory(DiContainer container, IAssetsAddressablesProvider assetsAddressablesProvider)
        {
            _container = container;
            _assetsAddressablesProvider = assetsAddressablesProvider;
        }

        public async Task<GameObject> CreateInstance(string path)
        {
            var prefab = await _assetsAddressablesProvider.GetAsset<GameObject>(path);
            var instance = _container.InstantiatePrefab(prefab);
            return instance;
        }

        public async Task<GameObject> CreateInstance(AssetReference path)
        {
            var prefab = await _assetsAddressablesProvider.GetAsset<GameObject>(path);
            var instance = _container.InstantiatePrefab(prefab);
            return instance;
        }
    }

    public interface IGameObjectFactory
    {
        public Task<GameObject> CreateInstance(string path);
        public Task<GameObject> CreateInstance(AssetReference path);
    }
}