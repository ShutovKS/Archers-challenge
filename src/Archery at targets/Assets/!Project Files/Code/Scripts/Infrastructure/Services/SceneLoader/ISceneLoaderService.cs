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
        Task<SceneInstance> LoadSceneAsync(string scenePath,
            LoadSceneMode loadSceneMode = LoadSceneMode.Single,
            CancellationToken cancellationToken = default);

        Task UnloadSceneAsync(string scenePath);
    }
}