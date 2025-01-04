#region

using System;
using System.Collections;
using UnityEngine;

#endregion

namespace Features.Projectile
{
    public class Arrow : MonoBehaviour, IProjectile
    {
        public event Action OnStopped;

        [SerializeField] private Transform tip;
        [SerializeField] private new Rigidbody rigidbody;
        [SerializeField] private Collider arrowCollider;
        [SerializeField] private GameObject tailVisualization;
        [SerializeField] private float timeToDestroyOnHit = 0f;
        [SerializeField] private float timeToDestroy = 10f;

        private bool _isInFlight;

        private void Awake()
        {
            IsFlight(false);
            IsPhysics(false);
            IsEffect(false);
        }

        public void Fire(float force)
        {
            IsFlight(true);
            IsPhysics(true);
            IsEffect(true);

            StartCoroutine(DestroyAfter(timeToDestroy));

            rigidbody.AddForce(tip.forward * force, ForceMode.Impulse);
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

                StartCoroutine(DestroyAfter(timeToDestroyOnHit));
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

        private IEnumerator DestroyAfter(float seconds)
        {
            yield return new WaitForSeconds(seconds);

            StopAllCoroutines();
            Destroy(gameObject);
        }
    }
}