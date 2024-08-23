using System;
using UnityEngine;

namespace Features.ColliderTools
{
    public class ColliderInteractionEnterTrigger : MonoBehaviour
    {
        public event Action<Collider> OnTriggered;

        private void OnTriggerEnter(Collider other)
        {
            OnTriggered?.Invoke(other);
        }
    }
}