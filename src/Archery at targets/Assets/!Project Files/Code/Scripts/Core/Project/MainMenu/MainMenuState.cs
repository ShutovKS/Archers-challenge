#region

using System;
using System.Threading.Tasks;
using Data.Level;
using Data.SceneContext;
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
using Logics.GameplayLevels;
using UI.Levels;
using UI.MainMenu;
using UnityEngine.SceneManagement;

#endregion

namespace Logics.Project
{
    [UsedImplicitly]
    public class MainMenuState : IState, IEnterable, IExitable
    {
        private readonly IProjectManagementService _projectManagementService;
        private readonly IStaticDataService _staticDataService;
        private readonly IWindowService _windowService;
        private readonly IPlayerService _playerService;
        private readonly ISceneLoaderService _sceneLoaderService;
        private readonly ISceneContextProvider _sceneContextProvider;
        private readonly IGameplayLevelsFactory _gameplayLevelsFactory;
        private readonly IXRSetupService _xrSetupService;
        private readonly IInteractorService _interactorService;

        private MainMenuSceneContextData _sceneContextData;
        private LevelData _levelData;
        private MainMenuUI _mainMenuUI;
        private LevelsUI _levelsUI;

        public MainMenuState(
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
            _playerService = playerService;
            _sceneLoaderService = sceneLoaderService;
            _sceneContextProvider = sceneContextProvider;
            _gameplayLevelsFactory = gameplayLevelsFactory;
            _xrSetupService = xrSetupService;
            _interactorService = interactorService;
        }

        public async void OnEnter()
        {
            _levelData = _staticDataService.GetLevelData<LevelData>("MainMenu");

            await _sceneLoaderService.LoadSceneAsync(_levelData.LocationScenePath, LoadSceneMode.Additive);

            _sceneContextData = _sceneContextProvider.Get<MainMenuSceneContextData>();

            ConfigurePlayer();

            await OpenMainMenu();
        }

        private void ConfigurePlayer()
        {
            _xrSetupService.SetXRMode(XRMode.VR);
            _interactorService.SetUpInteractor(HandType.Left, InteractorType.NearFar);
            _interactorService.SetUpInteractor(HandType.Right, InteractorType.NearFar);

            _playerService.SetPlayerPositionAndRotation(_sceneContextData.PlayerSpawnPoint.position,
                _sceneContextData.PlayerSpawnPoint.rotation);
        }

        private async Task OpenMainMenu()
        {
            _mainMenuUI = await _windowService.OpenInWorldAndGet<MainMenuUI>(
                WindowID.MainMenu,
                _sceneContextData.MainMenuScreenSpawnPoint.position,
                _sceneContextData.MainMenuScreenSpawnPoint.rotation
            );

            _mainMenuUI.OnInfiniteVRClicked += () => StartMode<InfiniteModeVRGameplayLevel>("InfiniteVR");
            _mainMenuUI.OnInfiniteMRClicked += () => StartMode<InfiniteModeMRGameplayLevel>("InfiniteMR");
            _mainMenuUI.OnLevelsClicked += async () => await OpenLevelsScreen();
            _mainMenuUI.OnExitClicked += ExitGame;
        }

        private async Task OpenLevelsScreen()
        {
            _levelsUI = await _windowService.OpenInWorldAndGet<LevelsUI>(
                WindowID.Levels,
                _sceneContextData.LevelsScreenSpawnPoint.position,
                _sceneContextData.LevelsScreenSpawnPoint.rotation
            );

            _levelsUI.OnBackClicked += () => CloseWindow(WindowID.Levels);
            _levelsUI.OnItemClicked += OnLevelSelected;

            var levelDatas = _staticDataService.GetLevelData<GameplayLevelData>();
            _levelsUI.SetItems(levelDatas);
        }

        private void OnLevelSelected(string levelId)
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
            CloseWindow(WindowID.MainMenu);
            UnloadLocation();
        }

        private void CloseWindow(WindowID windowId)
        {
            _windowService.Close(windowId);
        }

        private void UnloadLocation()
        {
            _sceneLoaderService.UnloadSceneAsync(_levelData.LocationScenePath);
        }
    }
}