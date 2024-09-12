#region

using Core.Project.Gameplay;
using Data.Configurations.Level;
using Infrastructure.Factories.GameplayLevels;
using Infrastructure.Providers.GlobalDataContainer;
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
        private readonly IGlobalContextProvider _globalContextProvider;

        private MainMenuUI _mainMenuUI;
        private LevelsUI _levelsUI;

        public MenuScreenState(
            IProjectManagementService projectManagementService,
            IStaticDataProvider staticDataProvider,
            IWindowService windowService,
            IGameplayLevelsFactory gameplayLevelsFactory,
            IGlobalContextProvider globalContextProvider)
        {
            _projectManagementService = projectManagementService;
            _staticDataProvider = staticDataProvider;
            _windowService = windowService;
            _globalContextProvider = globalContextProvider;
        }

        public void OnEnter()
        {
            InitializeMainMenuScreen();

            _mainMenuUI.Show();
        }

        private void InitializeMainMenuScreen()
        {
            _mainMenuUI = _windowService.Get<MainMenuUI>(WindowID.MainMenu);

            _mainMenuUI.OnInfiniteVRClicked += () => StartMode("InfiniteVR");

            _mainMenuUI.OnInfiniteMRClicked += () => StartMode("InfiniteMR");

            _mainMenuUI.OnLevelsClicked += OpenLevelsScreen;

            _mainMenuUI.OnExitClicked += ExitGame;
        }

        private void OpenLevelsScreen() => _projectManagementService.ChangeState<LevelsScreenState>();

        private void StartMode(string modeName)
        {
            var levelData = _staticDataProvider.GetLevelData<LevelData>(modeName);
            _globalContextProvider.GlobalContext.LevelData = levelData;
            
            _projectManagementService.ChangeState<ExitMenuAndLaunchGameplay>();
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