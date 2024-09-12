using System.Threading.Tasks;
using Features.Player;
using UnityEngine;

namespace Infrastructure.Services.Player
{
    public interface IPlayerService
    {
        GameObject Player { get; }
        PlayerContainer PlayerContainer { get; }

        Task InstantiatePlayerAsync();
        void SetPlayerPositionAndRotation(Vector3 position, Quaternion rotation);

        void SetPlayerActive(bool isActive);
    }
}