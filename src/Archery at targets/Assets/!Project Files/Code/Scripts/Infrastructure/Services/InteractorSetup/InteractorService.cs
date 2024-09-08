#region

using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

#endregion

namespace Infrastructure.Services.InteractorSetup
{
    [UsedImplicitly]
    public class InteractorService : IInteractorService, IInteractorProvider
    {
        public event Action<HandType, InteractorType, bool> OnInteractorSelect;

        private readonly Dictionary<HandType, List<IInteractor>> _handInteractors = new()
        {
            { HandType.Left, new List<IInteractor>() },
            { HandType.Right, new List<IInteractor>() }
        };

        private IInteractor _gazeInteractor;

        public void SetUpInteractor(HandType hand, InteractorType interactorType)
        {
            SetUpGazeInteractor(hand, interactorType);

            if (!_handInteractors.TryGetValue(hand, out var interactor))
            {
                throw new ArgumentOutOfRangeException(nameof(hand), hand, "Invalid side type");
            }

            SetUpInteractorForHand(interactor, hand, interactorType);
        }

        public bool IsInteractorActive(HandType hand, InteractorType interactorType)
        {
            if (interactorType.HasFlag(InteractorType.Gaze) && _gazeInteractor != null)
            {
                return _gazeInteractor.IsActive;
            }

            if (!_handInteractors.TryGetValue(hand, out var interactors))
            {
                throw new ArgumentOutOfRangeException(nameof(hand), hand, "Invalid side type");
            }

            return interactors.Any(interactor =>
                interactor.IsActive &&
                (interactor.InteractorType & interactorType) != 0
            );
        }

        private void SetUpInteractorForHand(List<IInteractor> interactors, HandType hand, InteractorType interactorType)
        {
            foreach (var interactor in interactors)
            {
                interactor.OnSelect -= OnInteractorSelectHandler;
                interactor.Deactivate();

                if ((interactor.InteractorType & interactorType) != 0)
                {
                    interactor.OnSelect += OnInteractorSelectHandler;
                    interactor.Activate();
                }
            }

            void OnInteractorSelectHandler(bool isSelected)
            {
                OnInteractorSelect?.Invoke(hand, interactorType, isSelected);
            }
        }

        private void SetUpGazeInteractor(HandType hand, InteractorType interactorType)
        {
            if (_gazeInteractor == null) return;

            _gazeInteractor.OnSelect -= OnInteractorSelectHandler;
            _gazeInteractor.Deactivate();

            if (interactorType.HasFlag(InteractorType.Gaze))
            {
                _gazeInteractor.OnSelect += OnInteractorSelectHandler;
                _gazeInteractor.Activate();
            }

            void OnInteractorSelectHandler(bool isSelected)
            {
                OnInteractorSelect?.Invoke(hand, InteractorType.Gaze, isSelected);
            }
        }

        public void Add(IInteractor interactor)
        {
            if (interactor == null) throw new ArgumentNullException(nameof(interactor));

            if (interactor.InteractorType.HasFlag(InteractorType.Gaze))
            {
                _gazeInteractor = interactor;
                return;
            }

            if (!_handInteractors.TryGetValue(interactor.HandType, out var handInteractor))
            {
                throw new ArgumentOutOfRangeException(nameof(interactor.HandType), interactor.HandType,
                    "Invalid side type");
            }

            handInteractor.Add(interactor);
        }

        public void Remove(IInteractor interactor)
        {
            if (interactor == null) throw new ArgumentNullException(nameof(interactor));

            if (interactor.InteractorType.HasFlag(InteractorType.Gaze))
            {
                if (_gazeInteractor == interactor)
                {
                    _gazeInteractor = null;
                }

                return;
            }

            if (!_handInteractors.TryGetValue(interactor.HandType, out var handInteractor))
            {
                throw new ArgumentOutOfRangeException(nameof(interactor.HandType), interactor.HandType,
                    "Invalid side type");
            }

            handInteractor.Remove(interactor);
        }
    }
}