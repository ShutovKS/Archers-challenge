using UnityEngine;

namespace BowAndArrows
{
    /// <summary>
    /// Скрипт, управляющий стрелой, которая может быть запущена с лука.
    /// </summary>
    public class Arrow : MonoBehaviour
    {
        [SerializeField] private float speed = 10f; // Начальная скорость стрелы
        [SerializeField] private Transform tip; // Точка на кончике стрелы
        [SerializeField] private Rigidbody rigidbody; // Ссылка на Rigidbody компонента стрелы
        [SerializeField] private GameObject tailVisualization; // Визуализация хвоста стрелы

        private bool _isInFlight; // Флаг, указывающий на то, что стрела находится в полете

        private void Awake()
        {
            rigidbody.isKinematic = true; // Делаем Rigidbody кинематичным, чтобы стрела не двигалась

            tailVisualization.SetActive(false); // Отключаем визуализацию хвоста стрелы
        }

        /// <summary>
        /// Запускает стрелу, используя заданное натяжение.
        /// </summary>
        /// <param name="pullAmount">Величина натяжения, нормализованная от 0 до 1.</param>
        public void Fire(float pullAmount)
        {
            _isInFlight = true; // Устанавливаем флаг, что стрела в полете

            // Делаем Rigidbody не кинематичным, чтобы физика начала действовать
            rigidbody.isKinematic = false;

            // Применяем начальную силу к стрелке
            rigidbody.AddForce(tip.forward * speed * pullAmount, ForceMode.Impulse);

            tailVisualization.SetActive(true); // Включаем визуализацию хвоста стрелы
        }

        private void FixedUpdate()
        {
            if (_isInFlight)
            {
                // Поворачиваем стрелу в направлении ее движения, чтобы она выглядела реалистично
                if (rigidbody.linearVelocity.sqrMagnitude > 0.1f) // Проверка на незначительную скорость
                {
                    transform.forward = rigidbody.linearVelocity.normalized;
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            // Обработка столкновения стрелы с объектом
            if (_isInFlight)
            {
                // Прекращаем движение стрелы
                StopArrow();

                // Отключаем физику и коллайдеры после столкновения
                DisablePhysics();
            }
        }

        /// <summary>
        /// Останавливает движение стрелы и закрепляет ее на месте столкновения.
        /// </summary>
        private void StopArrow()
        {
            _isInFlight = false; // Флаг, что стрела больше не в полете

            rigidbody.isKinematic = true; // Прекращаем движение стрелы

            tailVisualization.SetActive(false); // Отключаем визуализацию хвоста стрелы
        }

        /// <summary>
        /// Отключает физику и коллайдеры для стрелы после столкновения.
        /// </summary>
        private void DisablePhysics()
        {
            // Удаляем Rigidbody и Collider, чтобы предотвратить дальнейшую физику
            if (rigidbody != null)
            {
                Destroy(rigidbody);
            }

            if (TryGetComponent<Collider>(out var arrowCollider))
            {
                Destroy(arrowCollider);
            }

            // Опционально, можно удалить сам компонент Arrow, если стрела больше не нужна
            Destroy(this);
        }
    }
}