#region

using Infrastructure.Services.ProjectManagement;
using Infrastructure.Services.SceneLoader;
using JetBrains.Annotations;
using UnityEngine.SceneManagement;

#endregion

namespace Core.Project
{
    public class BootstrapState : IState, IEnterable
    {
        private readonly IProjectManagementService _projectManagementService;

        public BootstrapState(IProjectManagementService projectManagementService)
        {
            _projectManagementService = projectManagementService;
        }

        public void OnEnter()
        {
            LoadBootstrapScene();

            _projectManagementService.ChangeState<InitializationState>();
        }

        private void LoadBootstrapScene()
        {
            if (SceneManager.GetActiveScene().buildIndex != 0)
            {
                SceneManager.LoadScene(0);
            }
        }
    }
}