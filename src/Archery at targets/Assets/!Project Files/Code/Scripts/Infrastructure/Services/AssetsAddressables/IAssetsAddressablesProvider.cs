#region

using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

#endregion

namespace Infrastructure.Services.AssetsAddressables
{
    public interface IAssetsAddressablesProvider
    {
        Task<T> GetAsset<T>(string address) where T : Object;
        Task<T> GetAsset<T>(AssetReference assetReference) where T : Object;
        Task<List<T>> GetAssets<T>(IEnumerable<string> addresses) where T : Object;
        void CleanUp();
    }
}