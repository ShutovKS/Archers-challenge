#region

using System;
using Infrastructure.Providers.Interactor;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Zenject;

#endregion

namespace Infrastructure.Services.InteractorSetup
{
    public class InteractorControllerBase : MonoBehaviour, IInteractor
    {
        [field: SerializeField] public InteractorType InteractorType { get; private set; }
        [field: SerializeField] public HandType HandType { get; private set; }

        public event Action<bool> OnSelect;

        private IXRSelectInteractor _xrSelectInteractor;
        private IInteractorProvider _interactorProvider;
        private IInteractorService _interactorService;

        [Inject]
        public void Construct(IInteractorProvider interactorProvider, IInteractorService interactorService)
        {
            _interactorProvider = interactorProvider;
            _interactorService = interactorService;
        }

        private void Awake()
        {
            _xrSelectInteractor = GetComponent<IXRSelectInteractor>();

            if (_xrSelectInteractor != null)
            {
                _xrSelectInteractor.selectEntered.AddListener(OnSelectEntered);
                _xrSelectInteractor.selectExited.AddListener(OnSelectExited);
            }

            _interactorProvider.Add(this);
        }

        private void Start()
        {
            var isInteractorActive = _interactorService.IsInteractorActive(HandType, InteractorType);
            if (isInteractorActive)
            {
                Activate();
            }
            else
            {
                Deactivate();
            }
        }

        private void OnSelectEntered(SelectEnterEventArgs args)
        {
            OnSelect?.Invoke(true);
        }

        private void OnSelectExited(SelectExitEventArgs args)
        {
            OnSelect?.Invoke(false);
        }

        public bool IsActive => gameObject.activeSelf;

        public void Activate()
        {
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _interactorProvider.Remove(this);

            if (_xrSelectInteractor != null)
            {
                _xrSelectInteractor.selectEntered.RemoveListener(OnSelectEntered);
                _xrSelectInteractor.selectExited.RemoveListener(OnSelectExited);
            }
        }
    }
}