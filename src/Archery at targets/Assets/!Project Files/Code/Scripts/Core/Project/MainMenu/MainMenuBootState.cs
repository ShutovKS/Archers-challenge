using System.Threading.Tasks;
using Data.Level;
using Data.SceneContext;
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
using UnityEngine.SceneManagement;

namespace Core.Project.MainMenu
{
    public class MainMenuBootState : IState, IEnterable
    {
        private readonly IProjectManagementService _projectManagementService;
        private readonly IStaticDataService _staticDataService;
        private readonly IWindowService _windowService;
        private readonly IPlayerService _playerService;
        private readonly ISceneLoaderService _sceneLoaderService;
        private readonly ISceneContextProvider _sceneContextProvider;
        private readonly IXRSetupService _xrSetupService;
        private readonly IInteractorService _interactorService;

        public MainMenuBootState(
            IProjectManagementService projectManagementService,
            IStaticDataService staticDataService,
            IWindowService windowService,
            IPlayerService playerService,
            ISceneLoaderService sceneLoaderService,
            ISceneContextProvider sceneContextProvider,
            IXRSetupService xrSetupService,
            IInteractorService interactorService)
        {
            _projectManagementService = projectManagementService;
            _staticDataService = staticDataService;
            _windowService = windowService;
            _playerService = playerService;
            _sceneLoaderService = sceneLoaderService;
            _sceneContextProvider = sceneContextProvider;
            _xrSetupService = xrSetupService;
            _interactorService = interactorService;
        }

        public async void OnEnter()
        {
            await LoadMainMenuScene();
            ConfigurePlayer();
            await InstantiateMainMenuUI();
            await InstantiateLevelsUI();
            MoveToMainMenuState();
        }

        private async Task LoadMainMenuScene()
        {
            var levelData = _staticDataService.GetLevelData<LevelData>("MainMenu");
            await _sceneLoaderService.LoadSceneAsync(levelData.LocationScenePath, LoadSceneMode.Additive);
        }

        private void ConfigurePlayer()
        {
            _xrSetupService.SetXRMode(XRMode.VR);
            _interactorService.SetUpInteractor(HandType.Left, InteractorType.NearFar);
            _interactorService.SetUpInteractor(HandType.Right, InteractorType.NearFar);

            var playerSpawnPoint = _sceneContextProvider.Get<MainMenuSceneContextData>().PlayerSpawnPoint;
            _playerService.SetPlayerPositionAndRotation(playerSpawnPoint.position, playerSpawnPoint.rotation);
        }

        private async Task InstantiateMainMenuUI()
        {
            var screenSpawnPoint = _sceneContextProvider.Get<MainMenuSceneContextData>().MainMenuScreenSpawnPoint;

            var mainMenuUI = await _windowService.OpenInWorldAndGet<MainMenuUI>(WindowID.MainMenu,
                screenSpawnPoint.position,
                screenSpawnPoint.rotation);

            mainMenuUI.Hide();
        }

        private async Task InstantiateLevelsUI()
        {
            var screenSpawnPoint = _sceneContextProvider.Get<MainMenuSceneContextData>().LevelsScreenSpawnPoint;

            var levelsUI = await _windowService.OpenInWorldAndGet<LevelsUI>(WindowID.Levels,
                screenSpawnPoint.position,
                screenSpawnPoint.rotation);

            var gameplayLevelDatas = _staticDataService.GetLevelData<GameplayLevelData>();
            levelsUI.SetItems(gameplayLevelDatas);

            levelsUI.Hide();
        }

        private void MoveToMainMenuState()
        {
            _projectManagementService.ChangeState<MainMenuLogicState>();
        }
    }
}