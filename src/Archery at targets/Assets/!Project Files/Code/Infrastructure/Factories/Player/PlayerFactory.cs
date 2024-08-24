using System.Threading.Tasks;
using Extension;
using Features.Player;
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
        public PlayerContainer PlayerContainer { get; private set; }

        private readonly IGameObjectFactory _gameObjectFactory;

        public PlayerFactory(IGameObjectFactory gameObjectFactory)
        {
            _gameObjectFactory = gameObjectFactory;
        }


        public async Task<GameObject> CreatePlayer()
        {
            var player = await _gameObjectFactory.Instantiate(AssetsAddressableConstants.XR_ORIGIN_MR_RIG);

            PlayerContainer = player.GetComponent<PlayerContainer>();

            return player;
        }

        public async Task<GameObject> CreatePlayer(Vector3 position, Quaternion rotation)
        {
            var player = await CreatePlayer();

            player.SetPositionAndRotation(position, rotation);

            return player;
        }
    }
}