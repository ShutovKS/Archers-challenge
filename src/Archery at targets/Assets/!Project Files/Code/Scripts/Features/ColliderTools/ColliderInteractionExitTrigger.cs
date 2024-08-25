using System;
using UnityEngine;

namespace Features.ColliderTools
{
    public class ColliderInteractionExitTrigger : MonoBehaviour
    {
        public event Action<Collider> OnExited;

        private void OnTriggerExit(Collider other)
        {
            OnExited?.Invoke(other);
        }
    }
}