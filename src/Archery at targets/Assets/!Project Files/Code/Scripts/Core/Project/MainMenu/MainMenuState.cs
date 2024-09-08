using Infrastructure.Services.ProjectManagement;
using JetBrains.Annotations;

namespace Core.Project.MainMenu
{
    [UsedImplicitly]
    public class MainMenuState : IState, IEnterable
    {
        private readonly IProjectManagementService _projectManagementService;

        public MainMenuState(IProjectManagementService projectManagementService)
        {
            _projectManagementService = projectManagementService;
        }

        public void OnEnter()
        {
            MoveToMainMenuState();
        }

        private void MoveToMainMenuState()
        {
            _projectManagementService.ChangeState<MainMenuBootState>();
        }
    }
}