using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace BowAndArrows
{
    /// <summary>
    /// Управляет взаимодействием для тянущегося объекта в среде XR.
    /// Этот скрипт вычисляет величину натяжения на основе позиции интерактора
    /// и обновляет визуальное представление (линию и метку) натяжения.
    /// </summary>
    public class PullInteraction : XRBaseInteractable
    {
        /// <summary>
        /// Событие, которое срабатывает, когда действие натяжения завершено.
        /// Параметр float представляет конечную величину натяжения.
        /// </summary>
        public static event Action<float> PullActionReleased;

        // Поля для настройки точек взаимодействия и визуализации.
        [SerializeField] private Transform startTransform;
        [SerializeField] private Transform endTransform;
        [SerializeField] private Transform notchTransform;
        [SerializeField] private LineRenderer lineRenderer; // Ссылка на LineRenderer для визуализации натяжения
        [SerializeField] private AudioSource audioSourceString; // Ссылка на источник звука для звука натягивания тетивы
        [SerializeField] private float forwardArrowOffset; // Смещение стрелы вперед для корректного отображения

        private IXRSelectInteractor _pullingInteractor; // Интерактор, который в данный момент взаимодействует с объектом.
        private float _pullAmount; // Величина натяжения, представляющая собой нормализованное значение от 0 до 1.

        /// <summary>
        /// Настраивает слушатели взаимодействия при создании объекта.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            selectEntered.AddListener(OnSelectEnter);
            selectExited.AddListener(OnSelectExit);
        }

        /// <summary>
        /// Очищает слушатели взаимодействия для избежания утечек памяти при уничтожении объекта.
        /// </summary>
        protected override void OnDestroy()
        {
            base.OnDestroy();

            selectEntered.RemoveListener(OnSelectEnter);
            selectExited.RemoveListener(OnSelectExit);
        }

        /// <summary>
        /// Вызывается, когда интерактор начинает взаимодействовать с объектом.
        /// </summary>
        /// <param name="args">Аргументы события, содержащие информацию о интеракторе.</param>
        private void OnSelectEnter(SelectEnterEventArgs args)
        {
            _pullingInteractor = args.interactorObject;
        }

        /// <summary>
        /// Вызывается, когда интерактор прекращает взаимодействовать с объектом.
        /// </summary>
        /// <param name="args">Аргументы события, содержащие информацию о интеракторе.</param>
        private void OnSelectExit(SelectExitEventArgs args)
        {
            // Срабатывает событие завершения натяжения с текущей величиной натяжения.
            PullActionReleased?.Invoke(_pullAmount);

            // Сбрасывает состояние взаимодействия и визуализацию.
            _pullingInteractor = null;
            ResetPull();
        }

        /// <summary>
        /// Обновляет состояние интерактивного объекта. Этот метод вызывается в различных фазах обновления.
        /// </summary>
        /// <param name="updatePhase">Текущая фаза обновления.</param>
        public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
        {
            base.ProcessInteractable(updatePhase);

            if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic && isSelected && _pullingInteractor != null)
            {
                // Вычисляет величину натяжения на основе текущей позиции интерактора.
                _pullAmount = CalculatePull(_pullingInteractor.transform.position);
                UpdateString();
            }
        }

        /// <summary>
        /// Вычисляет величину натяжения на основе позиции интерактора относительно начальной и конечной точек.
        /// </summary>
        /// <param name="pullPosition">Текущая позиция интерактора.</param>
        /// <returns>Нормализованное значение, представляющее величину натяжения, ограниченное между 0 и 1.</returns>
        private float CalculatePull(Vector3 pullPosition)
        {
            var pullDirection = pullPosition - startTransform.position; // Направление от начала к интерактору.
            var targetDirection = endTransform.position - startTransform.position; // Направление от начала к концу.
            var maxLength = targetDirection.magnitude; // Максимальная длина натяжения.

            targetDirection.Normalize();
            var pullValue = Vector3.Dot(pullDirection, targetDirection) / maxLength; // Проекция натяжения на целевое направление.

            return Mathf.Clamp(pullValue, 0, 1);
        }

        /// <summary>
        /// Обновляет визуальное представление строки и метки на основе текущей величины натяжения.
        /// </summary>
        private void UpdateString()
        {
            var startLocalPos = startTransform.localPosition;
            var endLocalPos = endTransform.localPosition;
            var zPosition = Mathf.Lerp(startLocalPos.z, endLocalPos.z, _pullAmount); // Интерполяция между началом и концом.
            
            var notchPosition = notchTransform.localPosition;
            notchPosition.z = zPosition + forwardArrowOffset;
            notchTransform.localPosition = notchPosition;
            
            lineRenderer.SetPosition(1, new Vector3(startLocalPos.x, startLocalPos.y, zPosition));

            // Воспроизводит звук натяжения тетивы на основе величины натяжения.
            PlayStringSound(_pullAmount);
        }

        /// <summary>
        /// Сбрасывает взаимодействие натяжения в исходное состояние и обновляет визуализацию.
        /// </summary>
        private void ResetPull()
        {
            _pullAmount = 0;
            notchTransform.localPosition = new Vector3(notchTransform.localPosition.x, notchTransform.localPosition.y, 0);
            UpdateString();
        }

        /// <summary>
        /// Воспроизводит звук натяжения тетивы.
        /// </summary>
        private void PlayStringSound(float amount)
        {
            if (audioSourceString != null)
            {
                audioSourceString.volume = amount;
                if (_pullAmount > 0)
                {
                    if (!audioSourceString.isPlaying)
                    {
                        audioSourceString.Play();
                    }
                }
                else
                {
                    audioSourceString.Stop();
                }
            }
        }
    }
}