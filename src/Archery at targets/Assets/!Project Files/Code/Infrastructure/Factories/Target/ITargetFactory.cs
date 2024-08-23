using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Infrastructure.Factories.Target
{
    public interface ITargetFactory
    {
        event Action<string> TargetHit;
        Task<string> Instantiate(Vector3 position, Quaternion rotation);
        void Destroy(string targetId);
        void DestroyAll();
    }
}