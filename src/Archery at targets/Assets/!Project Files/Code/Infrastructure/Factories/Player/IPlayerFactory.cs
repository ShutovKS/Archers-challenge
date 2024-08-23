using System.Threading.Tasks;
using Infrastructure.Services.InteractorSetup;
using UnityEngine;

namespace Infrastructure.Factories.Player
{
    public interface IPlayerFactory
    {
        GameObject Player { get; }
        
        Camera PlayerCamera { get; }
        GameObject PlayerCameraGameObject { get; }
        
        Task<GameObject> CreatePlayer();
        Task<GameObject> CreatePlayer(Vector3 position, Quaternion rotation);
    }
}