using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

/// <summary>
/// Спавнит стрелы на луке и управляет их состоянием.
/// </summary>
public class ArrowSpawner : MonoBehaviour
{
    [SerializeField] private GameObject arrowPrefab; // Префаб для стрелы, которая будет спавниться
    [SerializeField] private Transform notchTransform; // Точка на луке, к которой крепится стрела
    [SerializeField] private XRGrabInteractable bow; // Ссылка на интерактивный объект лука
    [SerializeField] private AudioSource audioSourceShot; // Ссылка на источник звука для звука выстрела

    private bool _arrowNotched; // Указывает, находится ли стрела в состоянии на лука
    private GameObject _currentArrow; // Ссылка на текущую спавненную стрелу

    private void Awake()
    {
        // Подписка на событие освобождения стрелы
        PullInteraction.PullActionReleased += HandleArrowRelease;
    }

    private void OnDestroy()
    {
        // Отписка от события освобождения стрелы
        PullInteraction.PullActionReleased -= HandleArrowRelease;
    }

    private void Update()
    {
        // Проверка, выбран ли лук и нет ли уже зацепленной стрелы
        if (bow.isSelected && !_arrowNotched)
        {
            _arrowNotched = true;
            StartCoroutine(DelayedSpawnArrow());
        }

        // Проверка, если лук не выбран и есть текущая стрела
        if (!bow.isSelected && _currentArrow != null)
        {
            // Удаление текущей стрелы и сброс состояния зацепления
            Destroy(_currentArrow);
            ResetNotchState();
        }
    }

    /// <summary>
    /// Обрабатывает выпуск стрелы.
    /// </summary>
    /// <param name="pullAmount">Величина натяжения, нормализованная в диапазоне от 0 до 1.</param>
    private void HandleArrowRelease(float pullAmount)
    {
        if (pullAmount > 0 && _currentArrow != null)
        {
            // Запускает стрелу с рассчитанной величиной натяжения
            _currentArrow.transform.parent = null;
            _currentArrow.GetComponent<Arrow>().Fire(pullAmount);
            
            // Воспроизводит звук выстрела
            audioSourceShot?.Play();
        }

        // Сбрасывает состояние зацепления после выпуска стрелы
        ResetNotchState();
    }

    /// <summary>
    /// Спавнит новую стрелу с небольшой задержкой.
    /// </summary>
    private IEnumerator DelayedSpawnArrow()
    {
        yield return new WaitForSeconds(0.5f);

        if (_arrowNotched)
        {
            // Создает экземпляр префаба стрелы и крепит его к точке зацепления
            _currentArrow = Instantiate(arrowPrefab, notchTransform);
            _currentArrow.transform.localPosition = Vector3.zero; // Сброс локальной позиции, чтобы она совпала с точкой зацепления
            _currentArrow.transform.localRotation = Quaternion.identity; // Сброс локальной ориентации
        }
    }

    /// <summary>
    /// Сбрасывает состояние зацепления и очищает ссылку на текущую стрелу.
    /// </summary>
    private void ResetNotchState()
    {
        _arrowNotched = false;
        _currentArrow = null;
    }
}