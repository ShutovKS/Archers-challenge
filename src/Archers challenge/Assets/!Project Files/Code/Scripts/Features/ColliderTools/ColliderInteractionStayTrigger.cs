#region

using System;
using UnityEngine;

#endregion

namespace Features.ColliderTools
{
    public class ColliderInteractionStayTrigger : MonoBehaviour
    {
        public event Action<Collider> OnStay;

        private void OnTriggerStay(Collider other)
        {
            OnStay?.Invoke(other);
        }
    }
}