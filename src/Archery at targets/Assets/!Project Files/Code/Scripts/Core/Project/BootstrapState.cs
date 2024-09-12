#region

using Infrastructure.Services.ProjectManagement;
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
            _projectManagementService.ChangeState<InitializationState>();
        }
    }
}