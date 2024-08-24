using System.Threading.Tasks;
using Features.Player;
using Infrastructure.Services.InteractorSetup;
using UnityEngine;

namespace Infrastructure.Factories.Player
{
    public interface IPlayerFactory
    {
        PlayerContainer PlayerContainer { get; }

        Task<GameObject> CreatePlayer();
        Task<GameObject> CreatePlayer(Vector3 position, Quaternion rotation);
    }
}