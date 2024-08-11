using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace BowAndArrows
{
    /// <summary>
    /// Manages the interaction logic for the bowstring in an XR environment.
    /// </summary>
    public class Bowstring : XRBaseInteractable
    {
        public event Action<float> PullReleased;
        public event Action<bool> Selected;

        [Header("Transforms")] 
        [SerializeField] private Transform startTransform;
        [SerializeField] private Transform endTransform;
        [SerializeField] private Transform notchTransform;

        [Header("Components")] 
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private AudioSource audioSourceString;

        [Header("Settings")] 
        [SerializeField] private float forwardArrowOffset = 0.1f;

        private IXRSelectInteractor _pullingInteractor;
        private float _pullAmount;
        private bool _isLocked;

        public void LockSelect() => _isLocked = true;
        public void UnlockSelect() => _isLocked = false;

        protected override void Awake()
        {
            base.Awake();
            selectEntered.AddListener(OnSelectEnter);
            selectExited.AddListener(OnSelectExit);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            selectEntered.RemoveListener(OnSelectEnter);
            selectExited.RemoveListener(OnSelectExit);
        }

        private void OnSelectEnter(SelectEnterEventArgs args)
        {
            if (_isLocked) return;

            _pullingInteractor = args.interactorObject;
            Selected?.Invoke(true);
        }

        private void OnSelectExit(SelectExitEventArgs args)
        {
            if (_isLocked) return;

            ReleaseBow();
        }

        public void ReleaseBow()
        {
            PullReleased?.Invoke(_pullAmount);
            Selected?.Invoke(false);

            ResetPull();
        }

        public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
        {
            if (_isLocked) return;
         
            base.ProcessInteractable(updatePhase);

            if (isSelected && _pullingInteractor != null && updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
            {
                _pullAmount = CalculatePull(_pullingInteractor.transform.position);
                UpdateString();
            }
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

        private void UpdateString()
        {
            var zPosition = Mathf.Lerp(startTransform.localPosition.z, endTransform.localPosition.z, _pullAmount);
            var notchPosition = notchTransform.localPosition;
            notchPosition.z = zPosition + forwardArrowOffset;
            notchTransform.localPosition = notchPosition;

            lineRenderer.SetPosition(1,
                new Vector3(startTransform.localPosition.x, startTransform.localPosition.y, zPosition));

            PlayStringSound(_pullAmount);
        }

        private void ResetPull()
        {
            _pullAmount = 0;

            notchTransform.localPosition =
                new Vector3(notchTransform.localPosition.x, notchTransform.localPosition.y, 0);

            UpdateString();
        }

        private void PlayStringSound(float amount)
        {
            if (audioSourceString == null) return;

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