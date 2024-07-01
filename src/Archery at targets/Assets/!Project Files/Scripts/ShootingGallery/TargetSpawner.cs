using System;
using UnityEngine;

namespace ShootingGallery
{
    public abstract class TargetSpawner : MonoBehaviour
    {
        public event Action<int> OnTargetHitCountChanged;
        public int TargetCount { get; protected set; }

        protected abstract void SpawnTarget();

        protected virtual void OnTargetHit()
        {
            TargetCount++;
            OnTargetHitCountChanged?.Invoke(TargetCount);

            Debug.Log("Target hit: " + TargetCount);
            SpawnTarget();
        }
    }
}