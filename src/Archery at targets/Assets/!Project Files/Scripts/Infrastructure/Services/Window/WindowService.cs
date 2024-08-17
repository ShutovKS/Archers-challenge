using System;
using System.Threading.Tasks;
using Infrastructure.Factories.UI;
using Infrastructure.Services.AssetsAddressables;
using UnityEngine;

namespace Infrastructure.Services.Window
{
    public class WindowService : IWindowService
    {
        public WindowService(IUIFactory uiFactory)
        {
            _uiFactory = uiFactory;
        }

        private readonly IUIFactory _uiFactory;

        public async Task Open(WindowID windowID, Vector3? position = null, Quaternion? rotation = null)
        {
            await OpenWindow(windowID, position, rotation);
        }

        public async Task<T> OpenAndGet<T>(WindowID windowID, Vector3? position = null, Quaternion? rotation = null)
            where T : Component
        {
            await OpenWindow(windowID, position, rotation);

            return await _uiFactory.GetScreenComponent<T>(windowID);
        }

        public T Get<T>(WindowID windowID) where T : Component
        {
            return _uiFactory.GetScreenComponent<T>(windowID).Result;
        }

        private async Task OpenWindow(WindowID windowID, Vector3? position = null, Quaternion? rotation = null)
        {
            var windowsPath = GetWindowsPath(windowID);

            if (windowsPath == null)
            {
                throw new NullReferenceException($"No path for install windows: {windowID.ToString()}");
            }

            var instance = await _uiFactory.CreateScreen(windowsPath, windowID);

            if (position.HasValue)
            {
                instance.transform.position = position.Value;
            }

            if (rotation.HasValue)
            {
                instance.transform.rotation = rotation.Value;
            }
        }

        public void Close(WindowID windowID)
        {
            if (windowID == WindowID.Unknown)
            {
                Debug.Log("Unknown window id + " + windowID);
                return;
            }

            _uiFactory.DestroyScreen(windowID);
        }

        private static string GetWindowsPath(WindowID windowID)
        {
            return windowID switch
            {
                WindowID.MainMenu => AssetsAddressableConstants.MAIN_MENU_SCREEN_PREFAB,
                _ => null
            };
        }
    }
}