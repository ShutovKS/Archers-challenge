#region

using System;
using Features.Projectile;
using Infrastructure.Factories.Projectile;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

#endregion

namespace Features.Weapon
{
    [RequireComponent(typeof(IXRSelectInteractable))]
    public class Bow : MonoBehaviour, IWeapon
    {
        public event Action<bool> OnSelected;

        private bool _isSelected;

        public bool IsSelected
        {
            get => _isSelected;
            private set
            {
                _isSelected = value;
                OnSelected?.Invoke(value);
            }
        }

        [SerializeField] private Transform notchTransform;

        private IXRSelectInteractable _xrSelectInteractable;
        private IProjectileFactory _projectileFactory;
        private IProjectile _currentProjectile;
        private float _bowForce;

        public void SetUp(IProjectileFactory projectileFactory, float bowForce)
        {
            _projectileFactory = projectileFactory;
            _bowForce = bowForce;
        }

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

        public void Fire(float pullAmount)
        {
            if (_currentProjectile == null) return;

            _projectileFactory.GetInstance(_currentProjectile).transform.SetParent(null);

            _currentProjectile.Fire(pullAmount * _bowForce);

            _currentProjectile = null;
        }

        public async void Charge() => _currentProjectile = await _projectileFactory.Instantiate(notchTransform);

        public void Discharge() => _currentProjectile = null;

        private void OnBowTaken(SelectEnterEventArgs args) => IsSelected = true;

        private void OnBowDropped(SelectExitEventArgs args)
        {
            IsSelected = false;

            Discharge();
        }
    }
}