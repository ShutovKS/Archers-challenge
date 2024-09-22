#region

using System.Threading;
using System.Threading.Tasks;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

#endregion

namespace Infrastructure.Services.SceneLoader
{
    public interface ISceneLoaderService
    {
        Task<SceneInstance> LoadSceneAsync(string scenePath, LoadSceneMode loadSceneMode = LoadSceneMode.Single);
        Task UnloadSceneAsync(string scenePath);
    }
}