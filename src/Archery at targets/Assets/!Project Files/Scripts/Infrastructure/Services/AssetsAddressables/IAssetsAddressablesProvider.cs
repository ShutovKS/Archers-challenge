using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using Zenject;

namespace Infrastructure.Services.AssetsAddressables
{
    public interface IAssetsAddressablesProvider : IInitializable
    {
        new void Initialize();
        Task<T> GetAsset<T>(string address) where T : Object;
        Task<T> GetAsset<T>(AssetReference assetReference) where T : Object;
        Task<List<T>> GetAssets<T>(IEnumerable<string> addresses) where T : Object;
        Task<SceneInstance> LoadScene(string sceneAddress, LoadSceneMode loadSceneMode = LoadSceneMode.Single);
        void CleanUp();
    }
}