using Infrastructure.Services.GameSetup;
using Infrastructure.Services.ProjectManagement;

namespace Core.Project.MainMenu
{
    public class MainMenuState : IState, IEnterable
    {
        private readonly IProjectManagementService _projectManagementService;
        private readonly IMainMenuSetupService _mainMenuSetupService;

        public MainMenuState(IProjectManagementService projectManagementService,
            IMainMenuSetupService mainMenuSetupService)
        {
            _projectManagementService = projectManagementService;
            _mainMenuSetupService = mainMenuSetupService;
        }

        public async void OnEnter()
        {
            await _mainMenuSetupService.SetupMainMenuAsync();

            _projectManagementService.ChangeState<MenuScreenState>();
        }
    }
}