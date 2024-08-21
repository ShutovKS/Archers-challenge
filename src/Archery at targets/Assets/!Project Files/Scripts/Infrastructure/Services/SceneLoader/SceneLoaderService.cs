using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Infrastructure.Services.SceneLoader
{
    [UsedImplicitly]
    public class SceneLoaderService : ISceneLoaderService
    {
        private readonly Dictionary<string, AsyncOperationHandle<SceneInstance>> _sceneHandles = new();

        public bool IsSceneLoaded(string scenePath)
        {
            return _sceneHandles.ContainsKey(scenePath);
        }

        public IEnumerable<string> GetLoadedScenes()
        {
            return _sceneHandles.Keys;
        }

        public async Task LoadScenesAsync(IEnumerable<AssetReference> sceneReferences,
            LoadSceneMode loadSceneMode = LoadSceneMode.Additive, CancellationToken cancellationToken = default)
        {
            var loadTasks = sceneReferences
                .Select(sceneReference => LoadSceneAsync(sceneReference, loadSceneMode, cancellationToken))
                .ToList();

            await Task.WhenAll(loadTasks);
        }

        public async Task LoadScenesAsync(IEnumerable<string> scenePaths,
            LoadSceneMode loadSceneMode = LoadSceneMode.Additive, CancellationToken cancellationToken = default)
        {
            var loadTasks = scenePaths
                .Select(scenePath => LoadSceneAsync(scenePath, loadSceneMode, cancellationToken))
                .ToList();

            await Task.WhenAll(loadTasks);
        }

        public async Task<SceneInstance> LoadSceneAsync(AssetReference sceneReference,
            LoadSceneMode loadSceneMode = LoadSceneMode.Single, CancellationToken cancellationToken = default)
        {
            if (sceneReference == null) throw new ArgumentNullException(nameof(sceneReference));

            var scenePath = $"Scenes/Locations/{sceneReference.editorAsset.name}.unity";

            return await LoadSceneAsync(scenePath, loadSceneMode, cancellationToken);
        }

        public async Task<SceneInstance> LoadSceneAsync(string scenePath,
            LoadSceneMode loadSceneMode = LoadSceneMode.Single, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(scenePath))
                throw new ArgumentException("Scene path cannot be null or empty", nameof(scenePath));

            if (_sceneHandles.TryGetValue(scenePath, out var handle))
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    handle.Result.ActivateAsync();
                    return handle.Result;
                }

                _sceneHandles.Remove(scenePath);
            }

            try
            {
                var sceneHandle = Addressables.LoadSceneAsync(scenePath, loadSceneMode);
                _sceneHandles[scenePath] = sceneHandle;

                var sceneInstance = await sceneHandle.Task;

                if (!cancellationToken.IsCancellationRequested)
                {
                    return sceneInstance;
                }

                await UnloadSceneAsync(scenePath).ConfigureAwait(false);
                cancellationToken.ThrowIfCancellationRequested();

                return sceneInstance;
            }
            catch (Exception ex)
            {
                _sceneHandles.Remove(scenePath);
                Debug.LogError($"Failed to load scene at {scenePath}: {ex.Message}");
                throw;
            }
        }

        public async Task UnloadAllScenesAsync()
        {
            var unloadTasks = new List<Task>();

            foreach (var scenePath in GetLoadedScenes())
            {
                unloadTasks.Add(UnloadSceneAsync(scenePath));
            }

            await Task.WhenAll(unloadTasks);
        }

        public async Task UnloadSceneAsync(AssetReference sceneReference)
        {
            if (sceneReference == null) throw new ArgumentNullException(nameof(sceneReference));

            await UnloadSceneAsync(sceneReference.editorAsset.name);
        }

        public async Task UnloadSceneAsync(string scenePath)
        {
            if (string.IsNullOrEmpty(scenePath))
                throw new ArgumentException("Scene path cannot be null or empty", nameof(scenePath));

            if (_sceneHandles.TryGetValue(scenePath, out var handle))
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    await Addressables.UnloadSceneAsync(handle).Task.ConfigureAwait(false);
                }

                _sceneHandles.Remove(scenePath);
            }
            else
            {
                Debug.LogWarning($"Scene {scenePath} is not loaded or already unloaded.");
            }
        }

        public async Task ReloadSceneAsync(AssetReference sceneReference,
            LoadSceneMode loadSceneMode = LoadSceneMode.Single, CancellationToken cancellationToken = default)
        {
            if (sceneReference == null) throw new ArgumentNullException(nameof(sceneReference));

            await ReloadSceneAsync(sceneReference.editorAsset.name, loadSceneMode, cancellationToken);
        }

        public async Task ReloadSceneAsync(string scenePath, LoadSceneMode loadSceneMode = LoadSceneMode.Single,
            CancellationToken cancellationToken = default)
        {
            if (IsSceneLoaded(scenePath))
            {
                await UnloadSceneAsync(scenePath);
            }

            await LoadSceneAsync(scenePath, loadSceneMode, cancellationToken);
        }
    }
}