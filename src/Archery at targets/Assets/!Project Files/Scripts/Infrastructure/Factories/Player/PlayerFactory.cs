using System.Threading.Tasks;
using Extension;
using Infrastructure.Factories.GameObjects;
using Infrastructure.Services.AssetsAddressables;
using JetBrains.Annotations;
using UnityEngine;

namespace Infrastructure.Factories.Player
{
    [UsedImplicitly]
    public class PlayerFactory : IPlayerFactory
    {
        private readonly IGameObjectFactory _gameObjectFactory;
        public GameObject Player { get; private set; }

        public PlayerFactory(IGameObjectFactory gameObjectFactory)
        {
            _gameObjectFactory = gameObjectFactory;
        }

        public async Task<GameObject> CreatePlayer()
        {
            var player = await _gameObjectFactory.CreateInstance(AssetsAddressableConstants.XR_ORIGIN_MR_RIG);
            Player = player;
            return player;
        }

        public async Task<GameObject> CreatePlayer(Vector3 position, Quaternion rotation)
        {
            var player = await _gameObjectFactory.CreateInstance(AssetsAddressableConstants.XR_ORIGIN_MR_RIG);
            player.SetPositionAndRotation(position, rotation);
            Player = player;
            return player;
        }
    }
}