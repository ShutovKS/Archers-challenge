using System;
using UnityEngine;

namespace Features.PositionsContainer
{
    public abstract class PositionsContainer : MonoBehaviour
    {
        public abstract bool InfinitePositions { get; }
        public  event Action<bool> OnPositionsEnded;

        public abstract (Vector3 position, Quaternion rotation) GetTargetPosition();
    }
}