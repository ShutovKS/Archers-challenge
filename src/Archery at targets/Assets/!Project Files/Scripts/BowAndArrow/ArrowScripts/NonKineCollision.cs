using UnityEngine;
using UnityEngine.Events;

namespace BowAndArrow.ArrowScripts
{
    [RequireComponent(typeof(Rigidbody))]
    public class NonKineCollision : MonoBehaviour
    {
        [SerializeField] private UnityEvent collisionEvent;
        private Rigidbody _rigBod;

        private void Start()
        {
            _rigBod = GetComponent<Rigidbody>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (_rigBod.isKinematic)
            {
                return;
            }

            _rigBod.isKinematic = true;
            collisionEvent?.Invoke();
        }
    }
}