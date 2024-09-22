#region

using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

#endregion

namespace Infrastructure.Services.SceneLoader
{
    public interface ISceneLoaderService
    {
        Task<SceneInstance> LoadSceneAsync(AssetReference scenePath,
            LoadSceneMode loadSceneMode = LoadSceneMode.Single);

        Task UnloadSceneAsync(SceneInstance sceneInstance);
    }
}