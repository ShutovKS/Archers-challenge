#region

using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Providers.Interactor;

#endregion

namespace Infrastructure.Services.InteractorSetup
{
    public class InteractorService : IInteractorService, IInteractorProvider
    {
        public event Action<HandType, InteractorType, bool> OnInteractorSelect;

        private readonly Dictionary<HandType, List<IInteractor>> _handInteractors = new()
        {
            { HandType.Left, new List<IInteractor>() },
            { HandType.Right, new List<IInteractor>() }
        };

        private readonly List<IInteractor> _gazeInteractors = new();

        public void SetUpInteractor(HandType hand, InteractorType interactorType)
        {
            SetUpGazeInteractor(interactorType);

            if (_handInteractors.TryGetValue(hand, out var interactors))
            {
                SetUpInteractorForHand(interactors, hand, interactorType);
                return;
            }

            throw new ArgumentOutOfRangeException(nameof(hand), hand, "Invalid hand type");
        }

        public bool IsInteractorActive(HandType hand, InteractorType interactorType)
        {
            if (interactorType.HasFlag(InteractorType.Gaze) && _gazeInteractors.Any(interactor => interactor.IsActive))
            {
                return true;
            }

            if (_handInteractors.TryGetValue(hand, out var interactors))
            {
                return interactors.Any(interactor =>
                    interactor.IsActive && (interactor.InteractorType & interactorType) != 0
                );
            }

            throw new ArgumentOutOfRangeException(nameof(hand), hand, "Invalid hand type");
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

        private void SetUpGazeInteractor(InteractorType interactorType)
        {
            if (_gazeInteractors == null) return;

            foreach (var interactor in _gazeInteractors)
            {
                interactor.OnSelect -= OnInteractorSelectHandler;
                interactor.Deactivate();

                if (interactorType.HasFlag(InteractorType.Gaze))
                {
                    interactor.OnSelect += OnInteractorSelectHandler;
                    interactor.Activate();
                }
            }

            void OnInteractorSelectHandler(bool isSelected)
            {
                OnInteractorSelect?.Invoke(HandType.None, InteractorType.Gaze, isSelected); // No specific hand for gaze
            }
        }

        public void Add(IInteractor interactor)
        {
            if (interactor == null) throw new ArgumentNullException(nameof(interactor));

            if (interactor.InteractorType.HasFlag(InteractorType.Gaze))
            {
                _gazeInteractors.Add(interactor);
            }
            else if (_handInteractors.TryGetValue(interactor.HandType, out var handInteractor))
            {
                handInteractor.Add(interactor);
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(interactor.HandType), interactor.HandType,
                    "Invalid hand type");
            }
        }

        public void Remove(IInteractor interactor)
        {
            if (interactor == null) throw new ArgumentNullException(nameof(interactor));

            if (interactor.InteractorType.HasFlag(InteractorType.Gaze))
            {
                _gazeInteractors.Remove(interactor);
            }
            else if (_handInteractors.TryGetValue(interactor.HandType, out var handInteractor))
            {
                handInteractor.Remove(interactor);
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(interactor.HandType), interactor.HandType,
                    "Invalid hand type");
            }
        }
    }
}