using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace Features.Weapon
{
    [RequireComponent(typeof(IXRSelectInteractable))]
    public class Bow : MonoBehaviour, IWeapon
    {
        public event Action<bool> OnSelected;
        public event Action<bool> OnVisualizeProjectile;
        public event Action<float> OnPullReleased;
        
        public bool IsSelected => _xrSelectInteractable is { isSelected: true };

        private IXRSelectInteractable _xrSelectInteractable;

        private void Awake()
        {
            _xrSelectInteractable = GetComponent<IXRSelectInteractable>();
        }

        private void OnEnable()
        {
            _xrSelectInteractable.selectEntered.AddListener(OnBowTaken);
            _xrSelectInteractable.selectExited.AddListener(OnBowDropped);
        }

        private void OnDisable()
        {
            _xrSelectInteractable.selectEntered.RemoveListener(OnBowTaken);
            _xrSelectInteractable.selectExited.RemoveListener(OnBowDropped);
        }

        public void PullReleased(float pullAmount) => OnPullReleased?.Invoke(pullAmount);

        public void SelectBowstring(bool isSelected) => OnVisualizeProjectile?.Invoke(isSelected);

        private void OnBowTaken(SelectEnterEventArgs args) => OnSelected?.Invoke(true);
        private void OnBowDropped(SelectExitEventArgs args) => OnSelected?.Invoke(false);
    }
}