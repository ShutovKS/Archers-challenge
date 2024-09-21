using System.Threading.Tasks;
using Core.Project.MainMenu;
using Infrastructure.Services.Player;
using Infrastructure.Services.ProjectManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Project.Initialization
{
    public class InitializationState : IState, IEnterable
    {
        private readonly IProjectManagementService _projectManagementService;
        private readonly IPlayerService _playerService;

        public InitializationState(IProjectManagementService projectManagementService, IPlayerService playerService)
        {
            _projectManagementService = projectManagementService;
            _playerService = playerService;
        }

        public async void OnEnter()
        {
            await LoadBootstrapScene();

            await CreatePlayer();

            MoveToNextState();
        }

        private async Task LoadBootstrapScene()
        {
            if (SceneManager.GetActiveScene().buildIndex != 0)
            {
                await SceneManager.LoadSceneAsync(0);
            }
        }

        private async Task CreatePlayer() => await _playerService.InstantiatePlayerAsync();

        private void MoveToNextState() => _projectManagementService.ChangeState<MainMenuState>();
    }
}