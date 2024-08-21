using Infrastructure.Factories.Player;

namespace Infrastructure.Services.InteractorSetup
{
    public class InteractorSetupService : IInteractorSetupService
    {
        private readonly IPlayerFactory _playerFactory;
        private InteractorType _currentInteractorType = InteractorType.None;

        public InteractorSetupService(IPlayerFactory playerFactory)
        {
            _playerFactory = playerFactory;
        }

        public void SetInteractor(InteractorType interactorType)
        {
            if (_currentInteractorType == interactorType) return;

            _playerFactory.InteractorsManager.SetInteractorLeft(interactorType);
            _playerFactory.InteractorsManager.SetInteractorRight(interactorType);

            _currentInteractorType = interactorType;
        }
    }
}