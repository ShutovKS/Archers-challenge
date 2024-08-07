using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace BowAndArrows
{
    public class Bow : MonoBehaviour
    {
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

        private void OnBowTaken(SelectEnterEventArgs arg0)
        {
            Selected?.Invoke(true);
        }

        private void OnBowDropped(SelectExitEventArgs arg0)
        {
            Selected?.Invoke(false);
        }
    }
}