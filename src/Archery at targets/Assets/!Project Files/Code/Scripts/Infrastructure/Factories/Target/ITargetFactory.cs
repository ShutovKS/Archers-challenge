using System;
using System.Threading.Tasks;
using UnityEngine;

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