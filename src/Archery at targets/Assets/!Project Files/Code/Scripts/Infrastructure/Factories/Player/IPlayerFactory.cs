using System.Threading.Tasks;
using Features.Player;
using UnityEngine;

namespace Infrastructure.Factories.Player
{
    public interface IPlayerFactory
    {
        PlayerContainer PlayerContainer { get; }

        Task<GameObject> Instantiate(Vector3? position = null, Quaternion? rotation = null, Transform parent = null);
        
        void Destroy();
    }
}