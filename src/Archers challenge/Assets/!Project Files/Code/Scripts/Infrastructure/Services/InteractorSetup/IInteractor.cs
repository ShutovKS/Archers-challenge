#region

using System;

#endregion

namespace Infrastructure.Services.InteractorSetup
{
    public interface IInteractor
    {
        InteractorType InteractorType { get; }
        HandType HandType { get; }
        event Action<bool> OnSelect;
        bool IsActive { get; }
        void Activate();
        void Deactivate();
    }
}