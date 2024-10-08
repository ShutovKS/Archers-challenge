#region

using System.Threading.Tasks;
using Infrastructure.Services.Window;
using UnityEngine;

#endregion

namespace Infrastructure.Factories.UI
{
    public interface IUIFactory
    {
        Task<GameObject> CreateScreen(string assetAddress, WindowID windowId);
        T GetScreenComponent<T>(WindowID windowId) where T : Component;
        void DestroyScreen(WindowID windowId);
    }
}