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
        private readonly IXRSetupService _xrSetupService;

        public ARComponentsFactory(
            IPlayerFactory playerFactory,
            IGameObjectFactory gameObjectFactory,
            IXRSetupService xrSetupService)
        {
            _playerFactory = playerFactory;
            _gameObjectFactory = gameObjectFactory;
            _xrSetupService = xrSetupService;
        }

        public async Task CreateARComponent<T>() where T : Behaviour
        {
            var arComponentType = typeof(T);
            switch (arComponentType)
            {
                case not null when arComponentType == typeof(ARSession) ||
                                   arComponentType == typeof(ARInputManager):
                    await CreatedARSession();
                    break;

                case not null when arComponentType == typeof(ARCameraManager) ||
                                   arComponentType == typeof(ARCameraBackground):
                    var player = _playerFactory.Player;
                    var camera = player.GetComponentInChildren<Camera>().gameObject;
                    CreatedComponent<T>(camera);
                    break;

                case not null when arComponentType == typeof(ARPlaneManager) ||
                                   arComponentType == typeof(ARBoundingBoxManager) ||
                                   arComponentType == typeof(ARAnchorManager):
                    CreatedComponent<T>(_playerFactory.Player);
                    break;

                default:
                    Debug.LogWarning($"AR component of type {arComponentType} is not supported.");
                    break;
            }
        }

        private async Task CreatedARSession()
        {
            var instance = await _gameObjectFactory.CreateInstance(AssetsAddressableConstants.AR_SESSION);

            BindComponent(instance.GetComponent<ARSession>());
            BindComponent(instance.GetComponent<ARInputManager>());
        }

        private void CreatedComponent<T>(GameObject parent) where T : Behaviour
        {
            var component = parent.AddComponent<T>();
            BindComponent(component);
        }

        private void BindComponent<T>(T component) where T : Behaviour
        {
            _xrSetupService.AddXRComponent(component);
        }
    }
}