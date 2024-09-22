#region

using UnityEngine;

#endregion

namespace Features.PositionsContainer
{
    public abstract class PositionsContainer : MonoBehaviour
    {
        public abstract (Vector3 position, Quaternion rotation) GetTargetPosition();
    }
}