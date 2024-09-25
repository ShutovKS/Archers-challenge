#region

using System;
using System.Collections.Generic;
using Infrastructure.Services.Camera;
using Infrastructure.Services.Player;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using Object = UnityEngine.Object;

#endregion

namespace Infrastructure.Factories.ARComponents
{
    public class ARComponentsFactory : IARComponentsFactory
    {
        private readonly IPlayerService _playerFactory;
        private readonly ICameraService _cameraService;
        private readonly Dictionary<Type, Behaviour> _arComponents = new();

        public ARComponentsFactory(IPlayerService playerFactory, ICameraService cameraService)
        {
            _playerFactory = playerFactory;
            _cameraService = cameraService;
        }

        public T Create<T>() where T : Behaviour
        {
            var targetObject = typeof(T) switch
            {
                _ when typeof(T) == typeof(ARCameraManager) ||
                       typeof(T) == typeof(ARCameraBackground)
                    => _cameraService.Camera,

                _ when typeof(T) == typeof(ARSession) ||
                       typeof(T) == typeof(ARInputManager) ||
                       typeof(T) == typeof(ARPlaneManager) ||
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
                Object.Destroy(component);
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