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
using UI;
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
            await InstanceMainMenuScreen();

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

        private async Task InstanceMainMenuScreen()
        {
            _mainMenuUI = await _windowService.OpenAndGet<MainMenuUI>(
                WindowID.MainMenu,
                _sceneContextData.MainMenuScreenSpawnPoint.position,
                _sceneContextData.MainMenuScreenSpawnPoint.rotation
            );

            _mainMenuUI.AddButton("Infinite VR", StartInfiniteVR);
            _mainMenuUI.AddButton("Exit", ExitFromGame);
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
            CloseScreen();
            DestroyLocation();
        }

        private void CloseScreen()
        {
            _windowService.Close(WindowID.MainMenu);
        }

        private void DestroyLocation()
        {
            _sceneLoaderService.UnloadSceneAsync(_levelData.LocationScenePath);
        }
    }
}