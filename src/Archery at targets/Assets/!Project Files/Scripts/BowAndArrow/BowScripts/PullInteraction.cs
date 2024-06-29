using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace BowAndArrow.BowScripts
{
    public class PullInteraction : MonoBehaviour
    {
        [SerializeField] private Transform pullTransform;

        [SerializeField] private float maxDistance = 0.5f;

        [SerializeField] private InputActionReference triggerActionL;
        [SerializeField] private InputActionReference triggerActionR;

        [SerializeField] private UnityEvent enteredGrabTrigger;
        [SerializeField] private UnityEvent exitedGrabTrigger;
        [SerializeField] private UnityEvent startedGrabEvent;
        [SerializeField] private UnityEvent<float> endedGrabEvent;
        [SerializeField] private UnityEvent<float> pullEvent;

        private Vector3 _initGrabPos;
        private bool _canGrab;
        private bool _isGrabbing;

        private XRBaseInteractor _interactor;
        private Transform _grabberTransform;

        private void Start()
        {
            _initGrabPos = pullTransform.localPosition;
            _canGrab = false;
            _isGrabbing = false;

            triggerActionL.action.started += StartGrab;
            triggerActionL.action.canceled += EndGrab;
            triggerActionR.action.started += StartGrab;
            triggerActionR.action.canceled += EndGrab;
        }

        private void OnDestroy()
        {
            triggerActionL.action.started -= StartGrab;
            triggerActionL.action.canceled -= EndGrab;
            triggerActionR.action.started -= StartGrab;
            triggerActionR.action.canceled -= EndGrab;
        }

        private void Update()
        {
            if (_isGrabbing && _grabberTransform)
            {
                pullTransform.position = _grabberTransform.position;
                pullEvent?.Invoke(CalculatePullAmount());
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _interactor = other.GetComponent<XRBaseInteractor>();
                if (_interactor)
                {
                    _grabberTransform = _interactor.attachTransform;
                }

                enteredGrabTrigger?.Invoke();
                _canGrab = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _canGrab = false;
                exitedGrabTrigger?.Invoke();
            }
        }

        private void StartGrab(InputAction.CallbackContext ctx)
        {
            if (_isGrabbing || !_canGrab)
            {
                return;
            }

            _isGrabbing = true;
            startedGrabEvent?.Invoke();
        }

        private void EndGrab(InputAction.CallbackContext ctx)
        {
            if (!_isGrabbing)
            {
                return;
            }

            _isGrabbing = false;

            endedGrabEvent?.Invoke(CalculatePullAmount());

            pullTransform.localPosition = _initGrabPos;
            _grabberTransform = null;
        }

        private float CalculatePullAmount()
        {
            var pullAmount = Vector3.Distance(_initGrabPos, pullTransform.localPosition) / maxDistance;
            pullAmount = Mathf.Clamp(pullAmount, 0.0f, 1.0f);
            return pullAmount;
        }
    }
}