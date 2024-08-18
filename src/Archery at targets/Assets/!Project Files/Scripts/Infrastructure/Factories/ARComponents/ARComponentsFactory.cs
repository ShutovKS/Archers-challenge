using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Factories.GameObjects;
using Infrastructure.Factories.Player;
using Infrastructure.Services.AssetsAddressables;
using Infrastructure.Services.XRSetup;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace Infrastructure.Factories.ARComponents
{
    [UsedImplicitly]
    public class ARComponentsFactory : IARComponentsFactory
    {
        private readonly IPlayerFactory _playerFactory;
        private readonly IGameObjectFactory _gameObjectFactory;
        private readonly Dictionary<Type, Behaviour> _arComponents = new();

        public ARComponentsFactory(IPlayerFactory playerFactory, IGameObjectFactory gameObjectFactory)
        {
            _playerFactory = playerFactory;
            _gameObjectFactory = gameObjectFactory;
        }

        public async Task<T> Create<T>() where T : Behaviour
        {
            if (typeof(T) == typeof(ARSession) || typeof(T) == typeof(ARInputManager))
            {
                return await CreateARSessionComponent<T>();
            }

            var targetObject = typeof(T) switch
            {
                _ when typeof(T) == typeof(ARCameraManager) || typeof(T) == typeof(ARCameraBackground) =>
                    _playerFactory.Player.GetComponentInChildren<Camera>().gameObject,
                _ when typeof(T) == typeof(ARPlaneManager) ||
                       typeof(T) == typeof(ARBoundingBoxManager) ||
                       typeof(T) == typeof(ARAnchorManager) =>
                    _playerFactory.Player,
                _ => throw new NotSupportedException($"AR component of type {typeof(T)} is not supported.")
            };

            return CreateComponent<T>(targetObject);
        }

        public void Remove<T>() where T : Behaviour
        {
            if (typeof(T) == typeof(ARSession) || typeof(T) == typeof(ARInputManager))
            {
                RemoveARSessionComponent();
                return;
            }


            if (_arComponents.TryGetValue(typeof(T), out var component))
            {
                _arComponents.Remove(typeof(T));
                UnityEngine.Object.Destroy(component);
            }
        }

        public T Get<T>() where T : Behaviour
        {
            _arComponents.TryGetValue(typeof(T), out var component);
            return component as T;
        }

        private async Task<T> CreateARSessionComponent<T>() where T : Behaviour
        {
            var instance = await _gameObjectFactory.CreateInstance(AssetsAddressableConstants.AR_SESSION);

            if (typeof(T) == typeof(ARSession))
            {
                var component = instance.GetComponent<ARSession>();
                _arComponents.Add(typeof(T), component);
                return component as T;
            }

            if (typeof(T) == typeof(ARInputManager))
            {
                var component = instance.GetComponent<ARInputManager>();
                _arComponents.Add(typeof(T), component);
                return component as T;
            }

            throw new InvalidOperationException($"Unsupported AR session component type {typeof(T)}.");
        }

        private void RemoveARSessionComponent()
        {
            if (_arComponents.TryGetValue(typeof(ARSession), out var arSession))
            {
                _arComponents.Remove(typeof(ARSession));
                UnityEngine.Object.Destroy(arSession);
            }

            if (_arComponents.TryGetValue(typeof(ARInputManager), out var arInputManager))
            {
                _arComponents.Remove(typeof(ARInputManager));
                UnityEngine.Object.Destroy(arInputManager);
            }
        }

        private T CreateComponent<T>(GameObject parent) where T : Behaviour
        {
            var component = parent.AddComponent<T>();
            _arComponents.Add(typeof(T), component);
            return component;
        }
    }
}