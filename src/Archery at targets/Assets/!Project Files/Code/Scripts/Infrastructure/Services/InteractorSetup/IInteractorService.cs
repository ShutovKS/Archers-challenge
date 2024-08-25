using System;

namespace Infrastructure.Services.InteractorSetup
{
    public interface IInteractorService
    {
        event Action<HandType, InteractorType, bool> OnInteractorSelect;

        void SetUpInteractorForHand(HandType hand, InteractorType interactorType);

        bool IsInteractorActive(HandType hand, InteractorType interactorType);
    }
}