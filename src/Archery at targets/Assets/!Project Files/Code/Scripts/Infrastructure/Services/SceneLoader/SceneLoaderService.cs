#region

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

#endregion

namespace Infrastructure.Services.SceneLoader
{
    public class SceneLoaderService : ISceneLoaderService
    {
        private readonly Dictionary<string, AsyncOperationHandle<SceneInstance>> _sceneHandles = new();

        public async Task<SceneInstance> LoadSceneAsync(string scenePath,
            LoadSceneMode loadSceneMode = LoadSceneMode.Single, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(scenePath))
                throw new ArgumentException("Scene path cannot be null or empty", nameof(scenePath));

            if (_sceneHandles.TryGetValue(scenePath, out var handle))
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    var activateAsync = handle.Result.ActivateAsync();
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
    }
}