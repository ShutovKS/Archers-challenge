using System;
using System.Threading.Tasks;
using Data.Path;
using Infrastructure.Factories.UI;
using JetBrains.Annotations;
using UnityEngine;

namespace Infrastructure.Services.Window
{
    [UsedImplicitly]
    public class WindowService : IWindowService
    {
        public WindowService(IUIFactory uiFactory)
        {
            _uiFactory = uiFactory;
        }

        private readonly IUIFactory _uiFactory;

        public async Task Open(WindowID windowID, Vector3? position = null, Quaternion? rotation = null,
            Transform parent = null)
        {
            await OpenWindow(windowID, position, rotation, parent);
        }

        public async Task<T> OpenAndGet<T>(WindowID windowID, Vector3? position = null, Quaternion? rotation = null,
            Transform parent = null) where T : Component
        {
            await OpenWindow(windowID, position, rotation, parent);

            return await _uiFactory.GetScreenComponent<T>(windowID);
        }

        public T Get<T>(WindowID windowID) where T : Component
        {
            return _uiFactory.GetScreenComponent<T>(windowID).Result;
        }

        private async Task OpenWindow(WindowID windowID, Vector3? position = null, Quaternion? rotation = null,
            Transform parent = null)
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

            if (parent)
            {
                instance.transform.SetParent(parent);
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
                WindowID.HandMenu => AddressablesPaths.HAND_MENU_SCREEN_PREFAB,
                
                WindowID.MainMenu => AddressablesPaths.MAIN_MENU_SCREEN_PREFAB,
                
                WindowID.InformationDesk => AddressablesPaths.INFORMATION_DESK_SCREEN_PREFAB,
                _ => null
            };
        }
    }
}