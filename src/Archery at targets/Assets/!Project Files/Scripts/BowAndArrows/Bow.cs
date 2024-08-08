using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace BowAndArrows
{
    /// <summary>
    /// Управляет логикой взаимодействия лука в среде XR.
    /// </summary>
    public class Bow : MonoBehaviour
    {
        /// <summary>
        /// Событие, вызываемое при выборе или отмене выбора лука.
        /// </summary>
        /// <param name="isSelected">Выбран ли лук.</param>
        public event Action<bool> Selected;

        [SerializeField] private XRBaseInteractable interactable;

        private void OnEnable()
        {
            interactable.selectEntered.AddListener(OnBowTaken);
            interactable.selectExited.AddListener(OnBowDropped);
        }

        private void OnDisable()
        {
            interactable.selectEntered.RemoveListener(OnBowTaken);
            interactable.selectExited.RemoveListener(OnBowDropped);
        }

        private void OnBowTaken(SelectEnterEventArgs args)
        {
            Selected?.Invoke(true);
        }

        private void OnBowDropped(SelectExitEventArgs args)
        {
            Selected?.Invoke(false);
        }
    }
}