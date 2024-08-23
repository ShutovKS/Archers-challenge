using System.Threading.Tasks;
using Infrastructure.Services.Window;
using UnityEngine;

namespace Infrastructure.Factories.UI
{
    public interface IUIFactory
    {
        Task<GameObject> CreateScreen(string assetAddress, WindowID windowId);
        Task<T> GetScreenComponent<T>(WindowID windowId) where T : Component;
        void DestroyScreen(WindowID windowId);
    }
}