using System.Threading.Tasks;
using Features.Player;
using Infrastructure.Factories.Player;
using Infrastructure.Services.InteractorSetup;
using Infrastructure.Services.XRSetup;
using JetBrains.Annotations;
using UnityEngine;

namespace Infrastructure.Services.Player
{
    [UsedImplicitly]
    public class PlayerService : IPlayerService
    {
        public GameObject Player { get; private set; }
        public PlayerContainer PlayerContainer { get; private set; }

        private readonly IPlayerFactory _playerFactory;

        public PlayerService(IPlayerFactory playerFactory)
        {
            _playerFactory = playerFactory;
        }

        public async Task InstantiatePlayerAsync()
        {
            Player = await _playerFactory.Instantiate();
            PlayerContainer = Player.GetComponent<PlayerContainer>();
        }

        public void SetPlayerPositionAndRotation(Vector3 position, Quaternion rotation) =>
            Player.transform.SetPositionAndRotation(position, rotation);

        public void SetPlayerActive(bool isActive) => Player.SetActive(isActive);
    }
}