#region

using System.Threading.Tasks;
using Data.Constants.Paths;
using Infrastructure.Factories.UI;
using UnityEngine;

#endregion

namespace Infrastructure.Services.Window
{
    public class WindowService : IWindowService
    {
        public WindowService(IUIFactory uiFactory)
        {
            _uiFactory = uiFactory;
        }

        private readonly IUIFactory _uiFactory;

        public async Task OpenInWorld(WindowID windowID, Vector3 position, Quaternion rotation, Transform transform) =>
            await OpenWindow(windowID, position, rotation, transform);

        public async Task<T> OpenInWorldAndGet<T>(WindowID windowID, Vector3 position, Quaternion rotation,
            Transform transform) where T : Component
        {
            await OpenWindow(windowID, position, rotation, transform);

            return _uiFactory.GetScreenComponent<T>(windowID);
        }

        public T Get<T>(WindowID windowID) where T : Component => _uiFactory.GetScreenComponent<T>(windowID);

        private async Task OpenWindow(WindowID windowID, Vector3 position, Quaternion rotation, Transform transform)
        {
            var windowsPath = GetWindowsPath(windowID);

            var instance = await _uiFactory.CreateScreen(windowsPath, windowID);

            instance.transform.SetParent(transform);
            instance.transform.SetPositionAndRotation(position, rotation);
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

        private static string GetWindowsPath(WindowID windowID) => windowID switch
        {
            WindowID.HandMenu => AddressablesPaths.HAND_MENU_SCREEN_PREFAB,

            WindowID.MainMenu => AddressablesPaths.MAIN_MENU_SCREEN_PREFAB,
            WindowID.Levels => AddressablesPaths.LEVELS_SCREEN_PREFAB,

            WindowID.InformationDesk => AddressablesPaths.INFORMATION_DESK_SCREEN_PREFAB,

            _ => null
        };
    }
}