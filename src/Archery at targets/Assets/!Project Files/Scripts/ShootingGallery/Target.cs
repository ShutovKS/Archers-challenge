using System;
using UnityEngine;

namespace ShootingGallery
{
    public class Target : MonoBehaviour
    {
        public event Action OnHit;
        private bool _isHit;

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.gameObject.layer == LayerMask.NameToLayer("Arrow") && !_isHit)
            {
                _isHit = true;

                OnHit?.Invoke();

                Destroy(other.gameObject);
                Destroy(gameObject);
            }
        }
    }
}