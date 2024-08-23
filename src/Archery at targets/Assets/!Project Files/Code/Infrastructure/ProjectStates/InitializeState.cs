using System.Threading.Tasks;
using Infrastructure.Factories.Player;
using Infrastructure.Services.InteractorSetup;
using Infrastructure.Services.ProjectManagement;
using Infrastructure.Services.StaticData;
using Infrastructure.Services.XRSetup;
using JetBrains.Annotations;

namespace Infrastructure.ProjectStates
{
    [UsedImplicitly]
    public class InitializeState : IState, IEnterable
    {
        private readonly IPlayerFactory _playerFactory;
        private readonly IProjectManagementService _projectManagementService;

        public InitializeState(
            IPlayerFactory playerFactory,
            IProjectManagementService projectManagementService
        )
        {
            _playerFactory = playerFactory;
            _projectManagementService = projectManagementService;
        }

        public async void OnEnter()
        {
            await CreatePlayer();

            MoveToNextState();
        }

        private async Task CreatePlayer()
        {
            await _playerFactory.CreatePlayer();
        }

        private void MoveToNextState()
        {
            _projectManagementService.SwitchState<MainMenuState>();
        }
    }
}