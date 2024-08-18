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
        private readonly Dictionary<Type, Behaviour> _arComponents = new();

        public ARComponentsFactory(IPlayerFactory playerFactory)
        {
            _playerFactory = playerFactory;
        }

        public T Create<T>() where T : Behaviour
        {
            var targetObject = typeof(T) switch
            {
                _ when typeof(T) == typeof(ARCameraManager) ||
                       typeof(T) == typeof(ARCameraBackground)
                    => _playerFactory.Player.GetComponentInChildren<Camera>().gameObject,

                _ when typeof(T) == typeof(ARSession) ||
                       typeof(T) == typeof(ARInputManager) ||
                       typeof(T) == typeof(ARPlaneManager) ||
                       typeof(T) == typeof(ARBoundingBoxManager) ||
                       typeof(T) == typeof(ARMeshManager) ||
                       typeof(T) == typeof(ARAnchorManager)
                    => _playerFactory.Player,

                _ => throw new NotSupportedException($"AR component of type {typeof(T)} is not supported.")
            };

            return CreateComponent<T>(targetObject);
        }

        public void Remove<T>() where T : Behaviour
        {
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

        private T CreateComponent<T>(GameObject parent) where T : Behaviour
        {
            var component = parent.AddComponent<T>();
            _arComponents.Add(typeof(T), component);
            return component;
        }
    }
}