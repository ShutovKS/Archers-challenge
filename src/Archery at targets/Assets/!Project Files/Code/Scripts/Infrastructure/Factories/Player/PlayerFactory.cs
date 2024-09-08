#region

using System.Threading.Tasks;
using Data.Paths;
using Infrastructure.Factories.GameObjects;
using Infrastructure.Services.Camera;
using JetBrains.Annotations;
using UnityEngine;

#endregion

namespace Infrastructure.Factories.Player
{
    [UsedImplicitly]
    public class PlayerFactory : IPlayerFactory
    {
        private readonly IGameObjectFactory _gameObjectFactory;
        private readonly ICameraService _cameraService;

        public PlayerFactory(IGameObjectFactory gameObjectFactory, ICameraService cameraService)
        {
            _gameObjectFactory = gameObjectFactory;
            _cameraService = cameraService;
        }

        public async Task<GameObject> Instantiate(Vector3? position = null, Quaternion? rotation = null,
            Transform parent = null)
        {
            var instantiate = await _gameObjectFactory.InstantiateAsync(AddressablesPaths.XR_ORIGIN_MR_RIG,
                position, rotation, parent);

            _cameraService.SetCamera(instantiate.GetComponentInChildren<Camera>());

            return instantiate;
        }
    }
}