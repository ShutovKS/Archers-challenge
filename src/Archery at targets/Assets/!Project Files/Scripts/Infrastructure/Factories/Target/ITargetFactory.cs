using System;
using UnityEngine;

namespace Infrastructure.Factories.Target
{
    public interface ITargetFactory
    {
        event Action<string> TargetHit;
        void SpawnTargets(int count, Vector3 pointLimitationMin, Vector3 pointLimitationMax, Quaternion rotation);
        void Destroy(string targetId);
    }
}