using Infrastructure.Services.ProjectManagement;

namespace Core.Project.MainMenu
{
    public class MainMenuState : IState, IEnterable
    {
        private readonly IProjectManagementService _projectManagementService;

        public MainMenuState(IProjectManagementService projectManagementService)
        {
            _projectManagementService = projectManagementService;
        }

        public void OnEnter() => MoveToMainMenuState();

        private void MoveToMainMenuState() => _projectManagementService.ChangeState<BootState>();
    }
}