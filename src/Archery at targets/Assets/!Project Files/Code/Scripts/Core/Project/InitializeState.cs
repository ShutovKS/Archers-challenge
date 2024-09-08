#region

using System.Threading.Tasks;
using Infrastructure.Factories.Player;
using Infrastructure.Services.ProjectManagement;
using JetBrains.Annotations;

#endregion

namespace Logics.Project
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
            await _playerFactory.Instantiate();
        }

        private void MoveToNextState()
        {
            _projectManagementService.ChangeState<MainMenuState>();
        }
    }
}