#region

using System;

#endregion

namespace Infrastructure.Services.InteractorSetup
{
    public interface IInteractorService
    {
        event Action<HandType, InteractorType, bool> OnInteractorSelect;

        void SetUpInteractor(HandType hand, InteractorType interactorType);

        bool IsInteractorActive(HandType hand, InteractorType interactorType);
    }
}