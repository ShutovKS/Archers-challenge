using System.Linq;
using System.Threading.Tasks;
using Data.Level;
using Data.SceneContext;
using Extension;
using Infrastructure.Factories.LevelGameplay;
using Infrastructure.Factories.Player;
using Infrastructure.GameplayLevels;
using Infrastructure.Services.InteractorSetup;
using Infrastructure.Services.ProjectManagement;
using Infrastructure.Services.SceneContainer;
using Infrastructure.Services.SceneLoader;
using Infrastructure.Services.StaticData;
using Infrastructure.Services.Window;
using Infrastructure.Services.XRSetup;
using JetBrains.Annotations;
using UI.Levels;
using UI.MainMenu;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure.ProjectStates
{
    [UsedImplicitly]
    public class MainMenuState : IState, IEnterable, IExitable
    {
        private readonly IProjectManagementService _projectManagementService;
        private readonly IStaticDataService _staticDataService;
        private readonly IWindowService _windowService;
        private readonly IXRSetupService _xrSetupService;
        private readonly IInteractorService _interactorService;
        private readonly IPlayerFactory _playerFactory;
        private readonly ISceneLoaderService _sceneLoaderService;
        private readonly ISceneContextProvider _sceneContextProvider;
        private readonly IGameplayLevelFactory _gameplayLevelFactory;

        private MainMenuSceneContextData _sceneContextData;
        private LevelData _levelData;
        private MainMenuUI _mainMenuUI;
        private LevelsUI _levelsUI;

        public MainMenuState(
            IProjectManagementService projectManagementService,
            IStaticDataService staticDataService,
            IWindowService windowService,
            IXRSetupService xrSetupService,
            IInteractorService interactorService,
            IPlayerFactory playerFactory,
            ISceneLoaderService sceneLoaderService,
            ISceneContextProvider sceneContextProvider,
            IGameplayLevelFactory gameplayLevelFactory)
        {
            _projectManagementService = projectManagementService;
            _staticDataService = staticDataService;
            _windowService = windowService;
            _xrSetupService = xrSetupService;
            _interactorService = interactorService;
            _playerFactory = playerFactory;
            _sceneLoaderService = sceneLoaderService;
            _sceneContextProvider = sceneContextProvider;
            _gameplayLevelFactory = gameplayLevelFactory;
        }

        public async void OnEnter()
        {
            GetLevelData();
            await LoadLocation();
            GetSceneContextData();
            await OpenMainMenu();
            ConfigurePlayer();
        }

        private void GetLevelData()
        {
            _levelData = _staticDataService.GetLevelData<LevelData>("MainMenu");
        }

        private async Task LoadLocation()
        {
            await _sceneLoaderService.LoadSceneAsync(_levelData.LocationScenePath, LoadSceneMode.Additive);
        }

        private void GetSceneContextData()
        {
            _sceneContextData = _sceneContextProvider.Get<MainMenuSceneContextData>();
        }

        private async Task OpenMainMenu()
        {
            _mainMenuUI = await _windowService.OpenAndGet<MainMenuUI>(
                WindowID.MainMenu,
                _sceneContextData.MainMenuScreenSpawnPoint.position,
                _sceneContextData.MainMenuScreenSpawnPoint.rotation
            );

            _mainMenuUI.OnInfiniteVRClicked += StartInfiniteVR;
            _mainMenuUI.OnInfiniteMRClicked += () => { };
            _mainMenuUI.OnLevelsClicked += async () => await OpenLevelsScreen();
            _mainMenuUI.OnExitClicked += ExitFromGame;
        }

        private async Task OpenLevelsScreen()
        {
            _levelsUI = await _windowService.OpenAndGet<LevelsUI>(
                WindowID.Levels,
                _sceneContextData.LevelsScreenSpawnPoint.position,
                _sceneContextData.LevelsScreenSpawnPoint.rotation
            );

            _levelsUI.OnBackClicked += CloseLevelsScreen;
            _levelsUI.OnItemClicked += OnLevelItemClicked;

            var levelDatas = _staticDataService.GetLevelData<GameplayLevelData>();
            
            Debug.Log(levelDatas.Count());

            _levelsUI.SetItems(levelDatas);
        }

        private async void CloseLevelsScreen()
        {
            _levelsUI.OnBackClicked -= CloseLevelsScreen;
            _levelsUI.OnItemClicked -= OnLevelItemClicked;

            _windowService.Close(WindowID.Levels);

            await OpenMainMenu();
        }

        private void OnLevelItemClicked(string levelId)
        {
            // _gameplayLevelFactory.Create<VRGameplayLevel>();
            
            var levelData = _staticDataService.GetLevelData<LevelData>(levelId);

            _projectManagementService.SwitchState<GameplayState, LevelData>(levelData);
        }

        private void ConfigurePlayer()
        {
            _xrSetupService.SetXRMode(XRMode.VR);

            _interactorService.SetUpInteractorForHand(HandType.Left, InteractorType.NearFar);
            _interactorService.SetUpInteractorForHand(HandType.Right, InteractorType.NearFar);

            _playerFactory.PlayerContainer.Player.SetPositionAndRotation(_sceneContextData.PlayerSpawnPoint);
        }

        private void StartInfiniteVR()
        {
            _gameplayLevelFactory.Create<InfiniteModeVRGameplayLevel>();

            var levelData = _staticDataService.GetLevelData<LevelData>("InfiniteVR");

            _projectManagementService.SwitchState<GameplayState, LevelData>(levelData);
        }

        private void ExitFromGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            UnityEngine.Application.Quit();
#endif
        }

        public void OnExit()
        {
            CloseMainMenuScreen();
            DestroyLocation();
        }

        private void CloseMainMenuScreen()
        {
            _mainMenuUI.OnInfiniteVRClicked -= StartInfiniteVR;
            _mainMenuUI.OnInfiniteMRClicked -= () => { };
            _mainMenuUI.OnLevelsClicked -= async () => await OpenLevelsScreen();
            _mainMenuUI.OnExitClicked -= ExitFromGame;

            _windowService.Close(WindowID.MainMenu);
        }

        private void DestroyLocation()
        {
            _sceneLoaderService.UnloadSceneAsync(_levelData.LocationScenePath);
        }
    }
}