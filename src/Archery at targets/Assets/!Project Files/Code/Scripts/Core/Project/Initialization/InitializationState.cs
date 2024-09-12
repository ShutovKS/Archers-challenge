using Infrastructure.Services.ProjectManagement;

namespace Core.Project.Initialization
{
    public class InitializationState : IState, IEnterable
    {
        private readonly IProjectManagementService _projectManagementService;

        public InitializationState(IProjectManagementService projectManagementService)
        {
            _projectManagementService = projectManagementService;
        }

        public void OnEnter()
        {
            MoveToNextState();
        }

        private void MoveToNextState() => _projectManagementService.ChangeState<BootstrapSceneLoadingState>();
    }
}