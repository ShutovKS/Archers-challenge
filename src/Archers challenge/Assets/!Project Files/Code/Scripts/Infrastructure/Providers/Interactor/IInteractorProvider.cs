using Infrastructure.Services.InteractorSetup;

namespace Infrastructure.Providers.Interactor
{
    public interface IInteractorProvider
    {
        void Add(IInteractor interactor);
        void Remove(IInteractor interactor);
    }
}