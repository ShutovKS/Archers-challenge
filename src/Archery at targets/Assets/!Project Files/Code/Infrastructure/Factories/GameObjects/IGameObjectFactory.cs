using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Infrastructure.Factories.GameObjects
{
    public interface IGameObjectFactory
    {
        public Task<GameObject> Instantiate(string path);
        public Task<GameObject> Instantiate(AssetReference path);

        public Task<T> InstantiateAndGetComponent<T>(string path) where T : Component;
        public Task<T> InstantiateAndGetComponent<T>(AssetReference path) where T : Component;
    }
}