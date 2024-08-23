using System.Threading.Tasks;
using Extension;
using Infrastructure.Factories.GameObjects;
using Infrastructure.Services.AssetsAddressables;
using Infrastructure.Services.InteractorSetup;
using JetBrains.Annotations;
using UnityEngine;

namespace Infrastructure.Factories.Player
{
    [UsedImplicitly]
    public class PlayerFactory : IPlayerFactory
    {
        private readonly IGameObjectFactory _gameObjectFactory;
        public GameObject Player { get; private set; }

        public Camera PlayerCamera { get; private set; }
        public GameObject PlayerCameraGameObject => PlayerCamera.gameObject;

        public PlayerFactory(IGameObjectFactory gameObjectFactory)
        {
            _gameObjectFactory = gameObjectFactory;
        }

        public async Task<GameObject> CreatePlayer()
        {
            var player = await _gameObjectFactory.Instantiate(AssetsAddressableConstants.XR_ORIGIN_MR_RIG);

            Player = player;
            PlayerCamera = player.GetComponentInChildren<Camera>();

            return player;
        }

        public async Task<GameObject> CreatePlayer(Vector3 position, Quaternion rotation)
        {
            var player = await _gameObjectFactory.Instantiate(AssetsAddressableConstants.XR_ORIGIN_MR_RIG);
            player.SetPositionAndRotation(position, rotation);

            Player = player;
            PlayerCamera = player.GetComponentInChildren<Camera>();

            return player;
        }
    }
}