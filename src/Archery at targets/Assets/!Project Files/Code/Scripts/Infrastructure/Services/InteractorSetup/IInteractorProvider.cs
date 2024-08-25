namespace Infrastructure.Services.InteractorSetup
{
    public interface IInteractorProvider
    {
        void Add(IInteractor interactor);
        void Remove(IInteractor interactor);
    }
}