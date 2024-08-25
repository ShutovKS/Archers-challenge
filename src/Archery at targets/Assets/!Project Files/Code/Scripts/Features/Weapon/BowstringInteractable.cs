using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace Features.Weapon
{
    public class BowstringInteractable : XRBaseInteractable
    {
        [SerializeField] private Transform startTransform, endTransform, notchTransform;

        [Header("Components")] [SerializeField]
        private Bow bow;

        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private AudioSource audioSourceString;

        [Header("Settings")] [SerializeField] private Vector3 arrowOffset = new(0, 0, 0.1f);

        private float _pullAmount;
        private IXRSelectInteractor _pullingInteractor;
        private InteractionLayerMask _interactionLayerMaskOnInitialized;

        protected override void Awake()
        {
            base.Awake();

            _interactionLayerMaskOnInitialized = interactionLayers;
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            selectEntered.AddListener(Taken);
            selectExited.AddListener(Released);

            bow.OnSelected += HandleBowSelection;

            SetIsInteractable(bow.IsSelected);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            selectEntered.RemoveListener(Taken);
            selectExited.RemoveListener(Released);

            bow.OnSelected -= HandleBowSelection;
        }

        private void HandleBowSelection(bool isSelected)
        {
            SetIsInteractable(isSelected);

            if (!isSelected)
            {
                ResetPull();
            }
        }

        private void SetIsInteractable(bool isInteractable)
        {
            if (isInteractable)
            {
                interactionLayers = _interactionLayerMaskOnInitialized;
            }
            else
            {
                interactionLayers = 0;
            }
        }

        private void Taken(SelectEnterEventArgs args)
        {
            _pullingInteractor = args.interactorObject;

            bow.Charge();
        }

        private void Released(SelectExitEventArgs args)
        {
            bow.Fire(_pullAmount);

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
            var zPosition = Mathf.Lerp(startTransform.localPosition.z, endTransform.localPosition.z, _pullAmount);
            var notchPosition = notchTransform.localPosition;
            notchPosition.z = zPosition + arrowOffset.z;
            notchTransform.localPosition = notchPosition;

            lineRenderer.SetPosition(1, new Vector3(startTransform.localPosition.x, startTransform.localPosition.y, zPosition));
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