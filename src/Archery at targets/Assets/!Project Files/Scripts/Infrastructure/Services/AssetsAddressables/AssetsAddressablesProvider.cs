using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Infrastructure.Services.AssetsAddressables
{
    [UsedImplicitly]
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

        public async Task<SceneInstance> LoadScene(string sceneAddress,
            LoadSceneMode loadSceneMode = LoadSceneMode.Single)
        {
            if (_completedOperations.TryGetValue(sceneAddress, out var completed))
            {
                return (SceneInstance)completed.Result;
            }

            var handle = Addressables.LoadSceneAsync(sceneAddress, loadSceneMode);
            handle.Completed += h => { _completedOperations[sceneAddress] = h; };

            AddHandle(sceneAddress, handle);

            return await handle.Task;
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

        private void AddHandle(string key, AsyncOperationHandle handle)
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

            AddHandle(cacheKey, handle);

            return await handle.Task;
        }
    }
}