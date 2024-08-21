using System.Threading.Tasks;
using Data.Level;
using Extension;
using Infrastructure.Factories.Player;
using Infrastructure.ProjectStateMachine;
using Infrastructure.Services.InteractorSetup;
using Infrastructure.Services.StaticData;
using Infrastructure.Services.Window;
using Infrastructure.Services.XRSetup;
using Infrastructure.Services.XRSetup.TrackingMode;
using JetBrains.Annotations;
using UI;

namespace Infrastructure.ProjectStates
{
    [UsedImplicitly]
    public class MainMenuState : IState, IEnterable, IExitable
    {
        private readonly IProjectStateMachine _projectStateMachine;
        private readonly IStaticDataService _staticDataService;
        private readonly IWindowService _windowService;
        private readonly IXRSetupService _xrSetupService;
        private readonly IInteractorSetupService _interactorSetupService;
        private readonly IPlayerFactory _playerFactory;

        private MainMenuLevelData _levelData;
        private MainMenuUI _mainMenuUI;

        public MainMenuState(
            IProjectStateMachine projectStateMachine,
            IStaticDataService staticDataService,
            IWindowService windowService,
            IXRSetupService xrSetupService,
            IInteractorSetupService interactorSetupService,
            IPlayerFactory playerFactory)
        {
            _projectStateMachine = projectStateMachine;
            _staticDataService = staticDataService;
            _windowService = windowService;
            _xrSetupService = xrSetupService;
            _interactorSetupService = interactorSetupService;
            _playerFactory = playerFactory;
        }

        public async void OnEnter()
        {
            InitializeData();

            await InstanceMainMenuScreen();

            ConfigurePlayer();
        }

        private void InitializeData()
        {
            _levelData = _staticDataService.GetLevelData<MainMenuLevelData>("MainMenu");
        }

        private async Task InstanceMainMenuScreen()
        {
            _mainMenuUI = await _windowService.OpenAndGet<MainMenuUI>(
                WindowID.MainMenu,
                _levelData.ScreenSpawnPoint.Position,
                _levelData.ScreenSpawnPoint.Rotation.ToQuaternion()
            );
        }

        private void ConfigurePlayer()
        {
            _xrSetupService.SetXRMode(XRMode.VR);
            _xrSetupService.SetXRTrackingMode(new NoneTrackingMode());
            _xrSetupService.SetAnchorManagerState(false);

            _interactorSetupService.SetInteractor(InteractorType.Ray);

            _playerFactory.Player.SetPositionAndRotation(_levelData.PlayerSpawnPoint);
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
            _windowService.Close(WindowID.MainMenu);
        }
    }
}