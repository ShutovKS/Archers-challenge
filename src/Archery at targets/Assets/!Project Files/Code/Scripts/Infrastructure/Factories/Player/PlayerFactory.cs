#region

using System.Threading.Tasks;
using Data.Path;
using Features.Player;
using Infrastructure.Factories.GameObjects;
using JetBrains.Annotations;
using UnityEngine;

#endregion

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
            _instantiate = await _gameObjectFactory.Instantiate(AddressablesPaths.XR_ORIGIN_MR_RIG,
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