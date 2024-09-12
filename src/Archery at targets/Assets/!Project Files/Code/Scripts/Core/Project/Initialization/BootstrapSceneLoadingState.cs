using System.Threading.Tasks;
using Infrastructure.Services.ProjectManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Project.Initialization
{
    public class BootstrapSceneLoadingState : IState, IEnterable
    {
        private readonly IProjectManagementService _projectManagementService;

        public BootstrapSceneLoadingState(IProjectManagementService projectManagementService)
        {
            _projectManagementService = projectManagementService;
        }

        public async void OnEnter()
        {
            await LoadBootstrapScene();

            MoveToInstantiateState();
        }

        private async Task LoadBootstrapScene()
        {
            if (SceneManager.GetActiveScene().buildIndex != 0)
            {
                await SceneManager.LoadSceneAsync(0);
            }
        }

        private void MoveToInstantiateState() => _projectManagementService.ChangeState<PlayerInstantiateState>();
    }
}