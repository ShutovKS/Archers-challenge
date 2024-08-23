using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace Features.Weapon
{
    public class Bowstring : XRBaseInteractable
    {
        public event Action<float> OnPullReleased;
        public event Action<bool> OnSelected;

        [Header("Transforms")] 
        [SerializeField] private Transform startTransform;
        [SerializeField] private Transform endTransform;
        [SerializeField] private Transform notchTransform;

        [Header("Components")] 
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private AudioSource audioSourceString;
        [SerializeField] private Bow bow;

        [Header("Settings")] 
        [SerializeField] private Vector3 arrowOffset = new(0, 0, 0.1f);

        private IXRSelectInteractor _pullingInteractor;
        private float _pullAmount;
        private bool _isLocked;

        protected override void OnEnable()
        {
            base.OnEnable();

            selectEntered.AddListener(Taken);
            selectExited.AddListener(Released);
            
            bow.OnSelected += HandleBowSelection;
            OnPullReleased += bow.PullReleased;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            selectEntered.RemoveListener(Taken);
            selectExited.RemoveListener(Released);
            
            bow.OnSelected -= HandleBowSelection;
            OnPullReleased -= bow.PullReleased;
        }

        private void HandleBowSelection(bool isSelected)
        {
            _isLocked = isSelected;

            if (isSelected == false)
            {
                ResetPull();
            }
        }

        private void Taken(SelectEnterEventArgs args)
        {
            _pullingInteractor = args.interactorObject;

            OnSelected?.Invoke(true);
        }

        private void Released(SelectExitEventArgs args)
        {
            OnSelected?.Invoke(false);
            OnPullReleased?.Invoke(_pullAmount);

            ResetPull();
        }

        public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
        {
            base.ProcessInteractable(updatePhase);

            if (IsPulling(updatePhase))
            {
                UpdateString(_pullingInteractor.transform.position);
            }
        }

        private void ResetPull()
        {
            _pullAmount = 0;
            _pullingInteractor = null;

            UpdateVisuals();
            UpdateSound();
        }

        private bool IsPulling(XRInteractionUpdateOrder.UpdatePhase updatePhase)
        {
            return isSelected &&
                   _isLocked == false &&
                   _pullingInteractor != null &&
                   updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic;
        }

        private void UpdateString(Vector3 pullPosition)
        {
            _pullAmount = CalculatePull(pullPosition);

            UpdateVisuals();
            UpdateSound();
        }

        private float CalculatePull(Vector3 pullPosition)
        {
            var pullDirection = pullPosition - startTransform.position;
            var targetDirection = endTransform.position - startTransform.position;
            var maxLength = targetDirection.magnitude;
            targetDirection.Normalize();
            var pullValue = Vector3.Dot(pullDirection, targetDirection) / maxLength;

            return Mathf.Clamp(pullValue, 0, 1);
        }

        private void UpdateVisuals()
        {
            lineRenderer.SetPosition(1, new Vector3(
                startTransform.localPosition.x,
                startTransform.localPosition.y,
                Mathf.Lerp(startTransform.localPosition.z, endTransform.localPosition.z, _pullAmount))
            );

            notchTransform.localPosition += arrowOffset;
        }

        private void UpdateSound()
        {
            if (audioSourceString == null) return;

            audioSourceString.volume = _pullAmount;

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