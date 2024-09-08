#region

using Core.Gameplay;
using Data.Level;
using Infrastructure.Factories.GameplayLevels;
using Infrastructure.Services.InteractorSetup;
using Infrastructure.Services.Player;
using Infrastructure.Services.ProjectManagement;
using Infrastructure.Services.SceneContainer;
using Infrastructure.Services.SceneLoader;
using Infrastructure.Services.StaticData;
using Infrastructure.Services.Window;
using Infrastructure.Services.XRSetup;
using JetBrains.Annotations;
using UI.Levels;
using UI.MainMenu;

#endregion

namespace Core.Project.MainMenu
{
    [UsedImplicitly]
    public class MainMenuLogicState : IState, IEnterable, IExitable
    {
        private readonly IProjectManagementService _projectManagementService;
        private readonly IStaticDataService _staticDataService;
        private readonly IWindowService _windowService;
        private readonly ISceneLoaderService _sceneLoaderService;
        private readonly IGameplayLevelsFactory _gameplayLevelsFactory;

        private MainMenuUI _mainMenuUI;
        private LevelsUI _levelsUI;

        public MainMenuLogicState(
            IProjectManagementService projectManagementService,
            IStaticDataService staticDataService,
            IWindowService windowService,
            IPlayerService playerService,
            ISceneLoaderService sceneLoaderService,
            ISceneContextProvider sceneContextProvider,
            IGameplayLevelsFactory gameplayLevelsFactory,
            IXRSetupService xrSetupService,
            IInteractorService interactorService)
        {
            _projectManagementService = projectManagementService;
            _staticDataService = staticDataService;
            _windowService = windowService;
            _sceneLoaderService = sceneLoaderService;
            _gameplayLevelsFactory = gameplayLevelsFactory;
        }

        public void OnEnter()
        {
            InitializeMainMenuScreen();
            InitializeLevelsScreen();
            _mainMenuUI.Show();
        }

        private void InitializeMainMenuScreen()
        {
            _mainMenuUI = _windowService.Get<MainMenuUI>(WindowID.MainMenu);

            _mainMenuUI.OnInfiniteVRClicked += () => StartMode<InfiniteModeVRGameplayLevel>("InfiniteVR");

            _mainMenuUI.OnInfiniteMRClicked += () => StartMode<InfiniteModeMRGameplayLevel>("InfiniteMR");

            _mainMenuUI.OnLevelsClicked += _mainMenuUI.Hide;
            _mainMenuUI.OnLevelsClicked += _levelsUI.Show;

            _mainMenuUI.OnExitClicked += ExitGame;
        }

        private void InitializeLevelsScreen()
        {
            _levelsUI = _windowService.Get<LevelsUI>(WindowID.Levels);

            _levelsUI.OnBackClicked += _levelsUI.Hide;
            _levelsUI.OnBackClicked += _mainMenuUI.Show;

            _levelsUI.OnItemClicked += StartLevel;
        }

        private void StartLevel(string levelId)
        {
            var levelData = _staticDataService.GetLevelData<GameplayLevelData>(levelId);

            _projectManagementService.ChangeState<GameplayState, GameplayLevelData>(levelData);
        }

        private void StartMode<T>(string modeName) where T : IGameplayLevel
        {
            _gameplayLevelsFactory.Create<T>();
            var levelData = _staticDataService.GetLevelData<LevelData>(modeName);
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
            CloseWindows();
            UnloadLocation();
        }

        private void CloseWindows()
        {
            _windowService.Close(WindowID.MainMenu);
            _windowService.Close(WindowID.Levels);
        }

        private void UnloadLocation()
        {
            var levelData = _staticDataService.GetLevelData<LevelData>("MainMenu");
            _sceneLoaderService.UnloadSceneAsync(levelData.LocationScenePath);
        }
    }
}