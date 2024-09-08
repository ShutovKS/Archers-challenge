#region

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Factories.GameObjects;
using Infrastructure.Services.Window;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

#endregion

namespace Infrastructure.Factories.UI
{
    [UsedImplicitly]
    public class UIFactory : IUIFactory
    {
        private readonly IGameObjectFactory _gameObjectFactory;
        private readonly Dictionary<WindowID, GameObject> _screenInstances = new();

        public UIFactory(IGameObjectFactory gameObjectFactory)
        {
            _gameObjectFactory = gameObjectFactory;
        }

        public async Task<GameObject> CreateScreen(string assetAddress, WindowID windowId)
        {
            var instance = await _gameObjectFactory.InstantiateAsync(assetAddress);

            if (!_screenInstances.TryAdd(windowId, instance))
            {
                Debug.LogWarning($"A screen with WindowID {windowId} already exists. " +
                                 $"Replacing the existing display object.");
                Object.Destroy(_screenInstances[windowId]);
                _screenInstances[windowId] = instance;
            }

            return instance;
        }

        public T GetScreenComponent<T>(WindowID windowId) where T : Component
        {
            if (_screenInstances.TryGetValue(windowId, out var screenObject))
            {
                var screenComponent = screenObject.GetComponent<T>();
                if (screenComponent != null)
                {
                    return screenComponent;
                }

                Debug.LogError($"Screen component of type {typeof(T)} not found");
                return null;
            }

            Debug.LogError($"Screen with WindowID {windowId} not found");
            return null;
        }

        public void DestroyScreen(WindowID windowId)
        {
            if (!_screenInstances.Remove(windowId, out var screenObject))
                throw new Exception($"Screen with WindowID {windowId} not found");

            Object.Destroy(screenObject);
        }
    }
}