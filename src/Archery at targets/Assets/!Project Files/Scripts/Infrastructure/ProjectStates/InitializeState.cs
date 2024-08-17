using System.Threading.Tasks;
using Infrastructure.Factories.Player;
using Infrastructure.Services.ProjectStateMachine;

namespace Infrastructure.ProjectStates
{
    public class InitializeState : IState, IEnterable
    {
        private readonly IProjectStateMachineService _projectStateMachineService;
        private readonly IPlayerFactory _playerFactory;

        public InitializeState(IProjectStateMachineService projectStateMachineService, IPlayerFactory playerFactory)
        {
            _projectStateMachineService = projectStateMachineService;
            _playerFactory = playerFactory;
        }

        public async void OnEnter()
        {
            await CreatePlayer();

            MoveToMainMenu();
        }

        private async Task CreatePlayer()
        {
            await _playerFactory.CreatePlayer();
        }

        private void MoveToMainMenu()
        {
            _projectStateMachineService.SwitchState<MainMenuState>();
        }
    }
}