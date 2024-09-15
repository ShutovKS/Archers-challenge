#region

using System.Threading.Tasks;
using UnityEngine;

#endregion

namespace Infrastructure.Factories.Player
{
    public interface IPlayerFactory
    {
        Task<GameObject> Instantiate(Vector3? position = null, Quaternion? rotation = null, Transform parent = null);
    }
}