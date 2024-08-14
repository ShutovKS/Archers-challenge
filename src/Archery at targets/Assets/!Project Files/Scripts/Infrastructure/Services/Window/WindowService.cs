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

        public async Task Open(WindowID windowID)
        {
            await OpenWindow(windowID);
        }

        public async Task<T> OpenAndGetComponent<T>(WindowID windowID) where T : Component
        {
            await OpenWindow(windowID);

            var component = await _uiFactory.GetScreenComponent<T>(windowID);

            return component;
        }

        private async Task OpenWindow(WindowID windowID)
        {
            var windowsPath = GetWindowsPath(windowID);

            if (windowsPath == null)
            {
                throw new NullReferenceException($"No path for install windows: {windowID.ToString()}");
            }

            await _uiFactory.CreateScreen(windowsPath, windowID);
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