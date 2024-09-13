using Data.Configurations.Level;
using Data.Contexts.Scene;
using Infrastructure.Providers.SceneContainer;
using Infrastructure.Providers.StaticData;
using Infrastructure.Services.InteractorSetup;
using Infrastructure.Services.Player;
using Infrastructure.Services.ProjectManagement;
using Infrastructure.Services.Window;
using Infrastructure.Services.XRSetup;
using UI.Levels;

namespace Core.Project.MainMenu
{
    public class ConfigureState : IState, IEnterable
    {
        private readonly IPlayerService _playerService;
        private readonly IXRSetupService _xrSetupService;
        private readonly IInteractorService _interactorService;
        private readonly ISceneContextProvider _sceneContextProvider;
        private readonly IStaticDataProvider _staticDataProvider;
        private readonly IWindowService _windowService;
        private readonly IProjectManagementService _projectManagementService;

        public ConfigureState(
            IPlayerService playerService,
            IXRSetupService xrSetupService,
            IInteractorService interactorService,
            ISceneContextProvider sceneContextProvider,
            IStaticDataProvider staticDataProvider,
            IWindowService windowService,
            IProjectManagementService projectManagementService)
        {
            _playerService = playerService;
            _xrSetupService = xrSetupService;
            _interactorService = interactorService;
            _sceneContextProvider = sceneContextProvider;
            _staticDataProvider = staticDataProvider;
            _windowService = windowService;
            _projectManagementService = projectManagementService;
        }

        public void OnEnter()
        {
            ConfigurePlayer();
            
            ConfigureLevelsUI();

            MoveToNextState();
        }

        private void ConfigurePlayer()
        {
            _xrSetupService.SetXRMode(XRMode.VR);
            _interactorService.SetUpInteractor(HandType.Left, InteractorType.NearFar);
            _interactorService.SetUpInteractor(HandType.Right, InteractorType.NearFar);

            var playerSpawnPoint = _sceneContextProvider.Get<MainMenuSceneContextData>().PlayerSpawnPoint;
            _playerService.SetPlayerPositionAndRotation(playerSpawnPoint.position, playerSpawnPoint.rotation);
        }

        private void ConfigureLevelsUI()
        {
            var levelsUI = _windowService.Get<LevelsUI>(WindowID.Levels);

            var gameplayLevelDatas = _staticDataProvider.GetLevelData<LevelData>();
            levelsUI.SetItems(gameplayLevelDatas);
        }

        private void MoveToNextState() => _projectManagementService.ChangeState<MenuScreenState>();
    }
}