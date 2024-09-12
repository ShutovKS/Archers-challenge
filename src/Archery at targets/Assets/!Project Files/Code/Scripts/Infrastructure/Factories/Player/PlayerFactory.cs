#region

using System.Threading.Tasks;
using Data.Constants.Paths;
using Infrastructure.Factories.GameObjects;
using UnityEngine;

#endregion

namespace Infrastructure.Factories.Player
{
    public class PlayerFactory : IPlayerFactory
    {
        private readonly IGameObjectFactory _gameObjectFactory;

        public PlayerFactory(IGameObjectFactory gameObjectFactory)
        {
            _gameObjectFactory = gameObjectFactory;
        }

        public async Task<GameObject> Instantiate(Vector3? position = null, Quaternion? rotation = null,
            Transform parent = null)
        {
            var instantiate = await _gameObjectFactory.InstantiateAsync(AddressablesPaths.XR_ORIGIN_MR_RIG,
                position, rotation, parent);
            
            return instantiate;
        }
    }
}