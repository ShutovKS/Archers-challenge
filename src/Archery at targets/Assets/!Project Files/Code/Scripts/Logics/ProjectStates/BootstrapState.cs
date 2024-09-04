#region

using Infrastructure.Services.ProjectManagement;
using JetBrains.Annotations;
using UnityEngine.SceneManagement;

#endregion

namespace Logics.ProjectStates
{
    [UsedImplicitly]
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

            _projectManagementService.SwitchState<InitializeState>();
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