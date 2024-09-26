using Core.Project.Gameplay;
using Data.Configurations.Level;
using Infrastructure.Providers.StaticData;
using Infrastructure.Services.GameSetup;
using Infrastructure.Services.ProjectManagement;
using Infrastructure.Services.Window;
using UI.Levels;

namespace Core.Project.MainMenu
{
    public class LevelsScreenState : IState, IEnterable, IExitable
    {
        private readonly IWindowService _windowService;
        private readonly IStaticDataProvider _staticDataProvider;
        private readonly IProjectManagementService _projectManagementService;
        private readonly IMainMenuSetupService _mainMenuSetupService;

        private LevelsUI _levelsUI;

        public LevelsScreenState(IWindowService windowService, IStaticDataProvider staticDataProvider,
            IProjectManagementService projectManagementService, IMainMenuSetupService mainMenuSetupService)
        {
            _windowService = windowService;
            _staticDataProvider = staticDataProvider;
            _projectManagementService = projectManagementService;
            _mainMenuSetupService = mainMenuSetupService;
        }

        public void OnEnter()
        {
            InitializeLevelsScreen();

            _levelsUI.Show();
        }

        private void InitializeLevelsScreen()
        {
            _levelsUI = _windowService.Get<LevelsUI>(WindowID.Levels);

            _levelsUI.OnBackClicked += ExitLevelsScreen;
            _levelsUI.OnItemClicked += StartLevel;
        }

        private void StartLevel(string levelId)
        {
            _mainMenuSetupService.CleanupMainMenuAsync();

            var levelData = _staticDataProvider.GetLevelData<LevelData>(levelId);
            _projectManagementService.ChangeState<GameplayState, LevelData>(levelData);
        }

        private void ExitLevelsScreen() => _projectManagementService.ChangeState<MenuScreenState>();

        public void OnExit()
        {
            CleanupLevelsScreen();

            _levelsUI.Hide();
        }

        private void CleanupLevelsScreen()
        {
            _levelsUI.OnBackClicked -= ExitLevelsScreen;
            _levelsUI.OnItemClicked -= StartLevel;
        }
    }
}