#region

using System;
using System.Threading.Tasks;
using UnityEngine;

#endregion

namespace Infrastructure.Factories.Target
{
    public interface ITargetFactory
    {
        event Action<GameObject> TargetHit;
        Task<GameObject> Instantiate(Vector3 position, Quaternion rotation, Transform parent = null);
        void Destroy(GameObject targetId);
        void DestroyAll();
    }
}