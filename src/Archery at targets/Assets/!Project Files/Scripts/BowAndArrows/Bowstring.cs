using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace BowAndArrows
{
    /// <summary>
    /// Управляет взаимодействием для тетивы лука в среде XR.
    /// </summary>
    public class Bowstring : XRBaseInteractable
    {
        /// <summary>
        /// Событие, которое срабатывает, когда действие натяжения завершено.
        /// </summary>
        /// <param name=""> Параметр float представляет конечную величину натяжения. </param>
        public event Action<float> PullReleased;

        /// <summary>
        /// Событие, которое срабатывает, когда объект выбран или отменен.
        /// </summary>
        /// <param name=""> Параметр bool представляет состояние выбора. </param>
        public event Action<bool> Selected;

        [SerializeField] private Transform startTransform;
        [SerializeField] private Transform endTransform;
        [SerializeField] private Transform notchTransform;
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private AudioSource audioSourceString;
        [SerializeField] private float forwardArrowOffset;

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
            _pullingInteractor = null;

            PullReleased?.Invoke(_pullAmount);
            Selected?.Invoke(false);

            ResetPull();
        }


        public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
        {
            base.ProcessInteractable(updatePhase);

            if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic && isSelected && _pullingInteractor != null)
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
            var startLocalPos = startTransform.localPosition;
            var endLocalPos = endTransform.localPosition;
            var zPosition = Mathf.Lerp(startLocalPos.z, endLocalPos.z, _pullAmount);

            var notchPosition = notchTransform.localPosition;
            notchPosition.z = zPosition + forwardArrowOffset;
            notchTransform.localPosition = notchPosition;

            lineRenderer.SetPosition(1, new Vector3(startLocalPos.x, startLocalPos.y, zPosition));

            PlayStringSound(_pullAmount);
        }

        private void ResetPull()
        {
            _pullAmount = 0;

            var notchPosition = notchTransform.localPosition;
            notchPosition.z = 0;
            notchTransform.localPosition = notchPosition;

            UpdateString();
        }

        private void PlayStringSound(float amount)
        {
            if (audioSourceString == null)
            {
                return;
            }

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