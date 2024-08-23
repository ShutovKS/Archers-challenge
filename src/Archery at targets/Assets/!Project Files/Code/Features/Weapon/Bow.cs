using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace Features.Weapon
{
    public class Bow : MonoBehaviour, IWeapon
    {
        public event Action<bool> OnSelected;
        public event Action<float> OnPullReleased;

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

        public void PullReleased(float pullAmount) => OnPullReleased?.Invoke(pullAmount);

        private void OnBowTaken(SelectEnterEventArgs args) => OnSelected?.Invoke(true);
        private void OnBowDropped(SelectExitEventArgs args) => OnSelected?.Invoke(false);
    }
}