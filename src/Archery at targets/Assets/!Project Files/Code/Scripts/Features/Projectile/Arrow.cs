using System;
using UnityEngine;

namespace Features.Projectile
{
    public class Arrow : MonoBehaviour, IProjectile
    {
        public event Action OnStopped;

        [SerializeField] private float speed = 10f;
        [SerializeField] private Transform tip;
        [SerializeField] private new Rigidbody rigidbody;
        [SerializeField] private Collider arrowCollider;
        [SerializeField] private GameObject tailVisualization;

        private bool _isInFlight;

        private void Awake()
        {
            IsFlight(false);
            IsPhysics(false);
            IsEffect(false);
        }

        public void Fire(float pullAmount)
        {
            IsFlight(true);
            IsPhysics(true);
            IsEffect(true);

            rigidbody.AddForce(tip.forward * speed * pullAmount, ForceMode.Impulse);
        }

        private void FixedUpdate()
        {
            if (_isInFlight && rigidbody.linearVelocity.sqrMagnitude > 0.1f)
            {
                transform.forward = rigidbody.linearVelocity.normalized;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_isInFlight)
            {
                IsFlight(false);
                IsPhysics(false);
                IsEffect(false);
            }
        }

        private void IsFlight(bool isFlight)
        {
            if (_isInFlight && !isFlight)
            {
                OnStopped?.Invoke();
            }
            
            _isInFlight = isFlight;
        }

        private void IsPhysics(bool isPhysics)
        {
            arrowCollider.enabled = isPhysics;
            rigidbody.isKinematic = !isPhysics;
            rigidbody.detectCollisions = isPhysics;
        }

        private void IsEffect(bool isEffect)
        {
            tailVisualization.SetActive(isEffect);
        }
    }
}