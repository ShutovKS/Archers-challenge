using System.Threading.Tasks;
using Features.Player;
using Infrastructure.Factories.Player;
using Infrastructure.Services.Camera;
using Infrastructure.Services.InteractorSetup;
using Infrastructure.Services.XRSetup;
using JetBrains.Annotations;
using UnityEngine;

namespace Infrastructure.Services.Player
{
    public class PlayerService : IPlayerService
    {
        public GameObject Player { get; private set; }
        public PlayerContainer PlayerContainer { get; private set; }

        private readonly IPlayerFactory _playerFactory;
        private readonly ICameraService _cameraService;

        public PlayerService(IPlayerFactory playerFactory, ICameraService cameraService)
        {
            _playerFactory = playerFactory;
            _cameraService = cameraService;
        }

        public async Task InstantiatePlayerAsync()
        {
            Player = await _playerFactory.Instantiate();

            _cameraService.SetCamera(Player.GetComponentInChildren<UnityEngine.Camera>());

            PlayerContainer = Player.GetComponent<PlayerContainer>();
        }

        public void SetPlayerPositionAndRotation(Vector3 position, Quaternion rotation) =>
            Player.transform.SetPositionAndRotation(position, rotation);

        public void SetPlayerActive(bool isActive) => Player.SetActive(isActive);
    }
}