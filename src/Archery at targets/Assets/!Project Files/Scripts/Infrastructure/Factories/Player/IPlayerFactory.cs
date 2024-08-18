using System.Threading.Tasks;
using UnityEngine;

namespace Infrastructure.Factories.Player
{
    public interface IPlayerFactory
    {
        GameObject Player { get; }

        Task<GameObject> CreatePlayer();
        Task<GameObject> CreatePlayer(Vector3 position, Quaternion rotation);
    }
}