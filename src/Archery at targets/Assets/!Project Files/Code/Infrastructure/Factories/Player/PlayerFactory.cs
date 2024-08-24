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

        private GameObject _instantiate;

        public PlayerFactory(IGameObjectFactory gameObjectFactory)
        {
            _gameObjectFactory = gameObjectFactory;
        }

        public async Task<GameObject> Instantiate(Vector3? position = null, Quaternion? rotation = null,
            Transform parent = null)
        {
            _instantiate = await _gameObjectFactory.Instantiate(AssetsAddressableConstants.XR_ORIGIN_MR_RIG,
                position, rotation, parent);

            PlayerContainer = _instantiate.GetComponent<PlayerContainer>();

            return _instantiate;
        }

        public void Destroy()
        {
            _gameObjectFactory.Destroy(_instantiate);
        }
    }
}