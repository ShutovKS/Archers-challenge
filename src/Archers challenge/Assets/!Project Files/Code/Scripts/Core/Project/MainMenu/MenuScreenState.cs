#region

using Core.Project.Gameplay;
using Data.Configurations.Level;
using Infrastructure.Providers.StaticData;
using Infrastructure.Services.GameSetup;
using Infrastructure.Services.ProjectManagement;
using Infrastructure.Services.Window;
using UI.MainMenu;

#endregion

namespace Core.Project.MainMenu
{
    public class MenuScreenState : IState, IEnterable, IExitable
    {
        private readonly IProjectManagementService _projectManagementService;
        private readonly IStaticDataProvider _staticDataProvider;
        private readonly IWindowService _windowService;
        private readonly IMainMenuSetupService _mainMenuSetupService;

        private MainMenuUI _mainMenuUI;

        public MenuScreenState(
            IProjectManagementService projectManagementService,
            IStaticDataProvider staticDataProvider,
            IWindowService windowService,
            IMainMenuSetupService mainMenuSetupService)
        {
            _projectManagementService = projectManagementService;
            _staticDataProvider = staticDataProvider;
            _windowService = windowService;
            _mainMenuSetupService = mainMenuSetupService;
        }

        public void OnEnter()
        {
            InitializeMainMenuScreen();

            _mainMenuUI.Show();
        }

        private void InitializeMainMenuScreen()
        {
            _mainMenuUI = _windowService.Get<MainMenuUI>(WindowID.MainMenu);

            _mainMenuUI.OnInfiniteVRClicked += StartInfiniteVR;
            _mainMenuUI.OnInfiniteMRClicked += StartInfiniteMR;
            _mainMenuUI.OnLevelsClicked += OpenLevelsScreen;
            _mainMenuUI.OnExitClicked += ExitGame;
        }

        private void OpenLevelsScreen() => _projectManagementService.ChangeState<LevelsScreenState>();
        private void StartInfiniteVR() => StartMode("InfiniteVR");
        private void StartInfiniteMR() => StartMode("InfiniteMR");

        private void StartMode(string modeName)
        {
            _mainMenuSetupService.CleanupMainMenuAsync();

            var levelData = _staticDataProvider.GetLevelData<LevelData>(modeName);
            _projectManagementService.ChangeState<GameplayState, LevelData>(levelData);
        }

        private void ExitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            UnityEngine.Application.Quit();
#endif
        }

        public void OnExit()
        {
            CleanupMainMenuScreen();

            _mainMenuUI.Hide();
        }

        private void CleanupMainMenuScreen()
        {
            _mainMenuUI.OnInfiniteVRClicked -= StartInfiniteVR;
            _mainMenuUI.OnInfiniteMRClicked -= StartInfiniteMR;
            _mainMenuUI.OnLevelsClicked -= OpenLevelsScreen;
            _mainMenuUI.OnExitClicked -= ExitGame;
        }
    }
}