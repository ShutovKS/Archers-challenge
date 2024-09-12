#region

using System.Threading.Tasks;
using Core.Project.MainMenu;
using Infrastructure.Services.Player;
using Infrastructure.Services.ProjectManagement;

#endregion

namespace Core.Project
{
    public class PlayerInstantiateState : IState, IEnterable
    {
        private readonly IPlayerService _playerService;
        private readonly IProjectManagementService _projectManagementService;

        public PlayerInstantiateState(
            IPlayerService playerService,
            IProjectManagementService projectManagementService)
        {
            _playerService = playerService;
            _projectManagementService = projectManagementService;
        }

        public async void OnEnter()
        {
            await CreatePlayer();

            MoveToNextState();
        }

        private async Task CreatePlayer() => await _playerService.InstantiatePlayerAsync();

        private void MoveToNextState() => _projectManagementService.ChangeState<MainMenuState>();
    }
}