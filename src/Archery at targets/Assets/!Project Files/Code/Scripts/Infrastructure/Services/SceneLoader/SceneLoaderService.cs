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
        private readonly Dictionary<string, SceneInstance> _sceneHandles = new();

        public async Task<SceneInstance> LoadSceneAsync(string scenePath,
            LoadSceneMode loadSceneMode = LoadSceneMode.Single)
        {
            if (string.IsNullOrEmpty(scenePath))
                throw new ArgumentException("Scene path cannot be null or empty", nameof(scenePath));

            if (_sceneHandles.Remove(scenePath, out var handle))
            {
                await UnloadSceneAsync(scenePath);
            }

            return _sceneHandles[scenePath] = await Addressables.LoadSceneAsync(scenePath, loadSceneMode).Task;
        }

        public async Task UnloadSceneAsync(string scenePath)
        {
            if (string.IsNullOrEmpty(scenePath))
                throw new ArgumentException("Scene path cannot be null or empty", nameof(scenePath));

            if (_sceneHandles.Remove(scenePath, out var sceneInstance))
            {
                await Addressables.UnloadSceneAsync(sceneInstance).Task;
            }
            else
            {
                Debug.LogWarning($"Scene {scenePath} is not loaded or already unloaded.");
            }
        }
    }
}