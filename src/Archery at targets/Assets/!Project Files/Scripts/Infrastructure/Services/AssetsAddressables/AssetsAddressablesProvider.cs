using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Infrastructure.Services.AssetsAddressables
{
    public class AssetsAddressablesProvider : IAssetsAddressablesProvider
    {
        private readonly Dictionary<string, AsyncOperationHandle> _completedOperations = new();
        private readonly Dictionary<string, List<AsyncOperationHandle>> _handles = new();

        public void Initialize()
        {
            Addressables.InitializeAsync();
        }

        public async Task<T> GetAsset<T>(string address) where T : Object =>
            _completedOperations.TryGetValue(address, out var completed)
                ? completed.Result as T
                : await RunWinCacheOnComplete(Addressables.LoadAssetAsync<T>(address), address);

        public async Task<T> GetAsset<T>(AssetReference assetReference) where T : Object =>
            _completedOperations.TryGetValue(assetReference.AssetGUID, out var completed)
                ? completed.Result as T
                : await RunWinCacheOnComplete(Addressables.LoadAssetAsync<T>(assetReference), assetReference.AssetGUID);

        public async Task<List<T>> GetAssets<T>(IEnumerable<string> addresses) where T : Object
        {
            var loadTasks = addresses.Select(address => _completedOperations.TryGetValue(address, out var completed)
                ? Task.FromResult(completed.Result as T)
                : RunWinCacheOnComplete(Addressables.LoadAssetAsync<T>(address), address)).ToList();

            return (await Task.WhenAll(loadTasks)).ToList();
        }

        public void CleanUp()
        {
            foreach (var handle in _handles.Values.SelectMany(resourceHandles => resourceHandles))
            {
                Addressables.Release(handle);
            }

            _handles.Clear();
            _completedOperations.Clear();
        }

        private void AddHandle<T>(string key, AsyncOperationHandle handle) where T : class
        {
            if (!_handles.TryGetValue(key, out var resourceHandles))
            {
                resourceHandles = new List<AsyncOperationHandle>();
                _handles[key] = resourceHandles;
            }

            resourceHandles.Add(handle);
        }

        private async Task<T> RunWinCacheOnComplete<T>(AsyncOperationHandle<T> handle, string cacheKey) where T : Object
        {
            handle.Completed += h => { _completedOperations[cacheKey] = h; };

            AddHandle<T>(cacheKey, handle);

            return await handle.Task;
        }
    }
}