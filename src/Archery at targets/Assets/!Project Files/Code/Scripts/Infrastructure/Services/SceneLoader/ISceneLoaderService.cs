#region

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

#endregion

namespace Infrastructure.Services.SceneLoader
{
    public interface ISceneLoaderService
    {
        bool IsSceneLoaded(string scenePath);
        IEnumerable<string> GetLoadedScenes();

        Task LoadScenesAsync(IEnumerable<string> scenePaths,
            LoadSceneMode loadSceneMode = LoadSceneMode.Additive,
            CancellationToken cancellationToken = default);

        Task<SceneInstance> LoadSceneAsync(string scenePath,
            LoadSceneMode loadSceneMode = LoadSceneMode.Single,
            CancellationToken cancellationToken = default);

        Task UnloadAllScenesAsync();
        Task UnloadSceneAsync(string scenePath);

        Task ReloadSceneAsync(string scenePath,
            LoadSceneMode loadSceneMode = LoadSceneMode.Single,
            CancellationToken cancellationToken = default);
    }
}