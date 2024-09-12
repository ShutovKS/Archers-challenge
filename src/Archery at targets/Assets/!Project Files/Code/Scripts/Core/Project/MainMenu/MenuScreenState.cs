#region

using Core.Gameplay;
using Core.Project.Gameplay;
using Data.Configurations.Level;
using Infrastructure.Factories.GameplayLevels;
using Infrastructure.Providers.StaticData;
using Infrastructure.Services.ProjectManagement;
using Infrastructure.Services.Window;
using UI.Levels;
using UI.MainMenu;

#endregion

namespace Core.Project.MainMenu
{
    public class MenuScreenState : IState, IEnterable, IExitable
    {
        private readonly IProjectManagementService _projectManagementService;
        private readonly IStaticDataProvider _staticDataProvider;
        private readonly IWindowService _windowService;
        private readonly IGameplayLevelsFactory _gameplayLevelsFactory;

        private MainMenuUI _mainMenuUI;
        private LevelsUI _levelsUI;

        public MenuScreenState(
            IProjectManagementService projectManagementService,
            IStaticDataProvider staticDataProvider,
            IWindowService windowService,
            IGameplayLevelsFactory gameplayLevelsFactory)
        {
            _projectManagementService = projectManagementService;
            _staticDataProvider = staticDataProvider;
            _windowService = windowService;
            _gameplayLevelsFactory = gameplayLevelsFactory;
        }

        public void OnEnter()
        {
            InitializeMainMenuScreen();

            _mainMenuUI.Show();
        }

        private void InitializeMainMenuScreen()
        {
            _mainMenuUI = _windowService.Get<MainMenuUI>(WindowID.MainMenu);

            _mainMenuUI.OnInfiniteVRClicked += () => StartMode<InfiniteModeVRGameplayLevel>("InfiniteVR");

            _mainMenuUI.OnInfiniteMRClicked += () => StartMode<InfiniteModeMRGameplayLevel>("InfiniteMR");

            _mainMenuUI.OnLevelsClicked += OpenLevelsScreen;

            _mainMenuUI.OnExitClicked += ExitGame;
        }

        private void OpenLevelsScreen() => _projectManagementService.ChangeState<LevelsScreenState>();

        private void StartMode<T>(string modeName) where T : IGameplayLevel
        {
            _gameplayLevelsFactory.Create<T>();
            
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
            _mainMenuUI.Hide();
        }
    }
}