using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Factories.GameObjects;
using Infrastructure.Services.Window;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Infrastructure.Factories.UI
{
    public class UIFactory : IUIFactory
    {
        private readonly IGameObjectFactory _gameObjectFactory;

        private readonly Dictionary<WindowID, GameObject> _screenTypeToInstanceMap = new();
        private readonly Dictionary<Type, Component> _screenTypeToComponentMap = new();

        public UIFactory(IGameObjectFactory gameObjectFactory)
        {
            _gameObjectFactory = gameObjectFactory;
        }

        public async Task<GameObject> CreateScreen(string assetAddress, WindowID windowId)
        {
            var instance = await _gameObjectFactory.CreateInstance(assetAddress);

            if (_screenTypeToInstanceMap.TryAdd(windowId, instance))
            {
                return instance;
            }

            Debug.LogWarning($"A screen with WindowID {windowId} already exists. Replacing an existing display object.");

            Object.Destroy(_screenTypeToInstanceMap[windowId]);

            _screenTypeToInstanceMap[windowId] = instance;

            return instance;
        }

        public Task<T> GetScreenComponent<T>(WindowID windowId) where T : Component
        {
            if (_screenTypeToInstanceMap.TryGetValue(windowId, out var screenObject))
            {
                var screenComponent = screenObject.GetComponent<T>();

                if (screenComponent == null)
                {
                    Debug.LogError($"Screen component of type {typeof(T)} not found");

                    return Task.FromResult<T>(null);
                }

                _screenTypeToComponentMap[typeof(T)] = screenComponent;

                return Task.FromResult(screenComponent);
            }

            Debug.LogError($"Screen with WindowID {windowId} not found");

            return Task.FromResult<T>(null);
        }

        public void DestroyScreen(WindowID windowId)
        {
            if (!_screenTypeToInstanceMap.Remove(windowId, out var screenObject))
            {
                throw new Exception($"Screen with WindowID {windowId} not found");
            }

            Object.Destroy(screenObject);
        }
    }
}