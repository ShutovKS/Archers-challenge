#region

using Core.Project.Initialization;
using Infrastructure.Services.ProjectManagement;

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

        public void OnEnter() => MoveToNextState();

        private void MoveToNextState() => _projectManagementService.ChangeState<InitializationState>();
    }
}