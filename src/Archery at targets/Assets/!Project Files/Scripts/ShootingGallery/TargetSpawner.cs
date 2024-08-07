using System;
using UnityEngine;

namespace ShootingGallery
{
    public abstract class TargetSpawner : MonoBehaviour
    {
        public event Action TargetHit;
        public abstract void SpawnTarget();

        protected void OnTargetHit()
        {
            TargetHit?.Invoke();
        }
    }
}