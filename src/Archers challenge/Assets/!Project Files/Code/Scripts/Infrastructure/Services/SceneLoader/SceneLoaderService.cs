#region

using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

#endregion

namespace Infrastructure.Services.SceneLoader
{
    public class SceneLoaderService : ISceneLoaderService
    {
        public async Task<SceneInstance> LoadSceneAsync(AssetReference scenePath, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
        {
            var sceneInstance = await scenePath.LoadSceneAsync(loadSceneMode, true).Task;
            return sceneInstance;
        }

        public async Task UnloadSceneAsync(SceneInstance sceneInstance) =>
            await Addressables.UnloadSceneAsync(sceneInstance).Task;
    }
}