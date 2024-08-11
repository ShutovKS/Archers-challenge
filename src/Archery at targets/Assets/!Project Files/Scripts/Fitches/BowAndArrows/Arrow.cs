using UnityEngine;

namespace Fitches.BowAndArrows
{
    /// <summary>
    /// Управляет логикой стрелок, включая поведение полета и столкновения.
    /// </summary>
    public class Arrow : MonoBehaviour
    {
        [SerializeField] private float speed = 10f;
        [SerializeField] private Transform tip;
        [SerializeField] private new Rigidbody rigidbody;
        [SerializeField] private GameObject tailVisualization;

        private bool _isInFlight;

        private void Awake()
        {
            rigidbody.isKinematic = true;
            tailVisualization.SetActive(false);
        }

        /// <summary>
        /// Выпускает стрелу с указанной силой притяжения.
        /// </summary>
        /// <param name="pullAmount">Нормализованная величина вытягивания, от 0 до 1.</param>
        public void Fire(float pullAmount)
        {
            transform.SetParent(null);

            _isInFlight = true;
            rigidbody.isKinematic = false;
            rigidbody.AddForce(tip.forward * speed * pullAmount, ForceMode.Impulse);

            tailVisualization.SetActive(true);
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
                StopArrow();
                DisablePhysics();
            }
        }

        private void StopArrow()
        {
            _isInFlight = false;
            rigidbody.isKinematic = true;
            tailVisualization.SetActive(false);
        }

        private void DisablePhysics()
        {
            if (rigidbody != null)
            {
                Destroy(rigidbody);
            }

            var arrowCollider = GetComponent<Collider>();
            if (arrowCollider != null)
            {
                Destroy(arrowCollider);
            }

            Destroy(this);
        }
    }
}