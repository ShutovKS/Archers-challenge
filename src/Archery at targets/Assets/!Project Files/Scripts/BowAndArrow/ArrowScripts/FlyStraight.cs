using UnityEngine;

namespace BowAndArrow.ArrowScripts
{
    [RequireComponent(typeof(Rigidbody))]
    public class FlyStraight : MonoBehaviour
    {
        private Rigidbody _rigBod;
        private Transform _trans;

        private void Start()
        {
            _rigBod = GetComponent<Rigidbody>();
            _trans = transform;
        }

        private void FixedUpdate()
        {
            if (_rigBod.isKinematic)
            {
                return;
            }

            SetDirection();
        }

        private void SetDirection()
        {
            if (_rigBod.linearVelocity.z > 0.5f)
            {
                _trans.forward = _rigBod.linearVelocity;
            }
        }
    }
}