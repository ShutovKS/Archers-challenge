using System.Threading.Tasks;
using Data.Level;
using Extension;
using Infrastructure.Factories.GameObjects;
using Infrastructure.Factories.Player;
using Infrastructure.Services.AssetsAddressables;
using Infrastructure.Services.ProjectStateMachine;
using Infrastructure.Services.StaticData;
using Infrastructure.Services.Window;
using UI;

namespace Infrastructure.ProjectStates
{
    public class MainMenuLoadingState : IState, IEnterable
    {
        private readonly IPlayerFactory _playerFactory;
        private readonly IGameObjectFactory _gameObjectFactory;
        private readonly IWindowService _windowService;
        private readonly IStaticDataService _staticDataService;
        private readonly IProjectStateMachineService _projectStateMachineService;
        private readonly IAssetsAddressablesProvider _assetsAddressablesProvider;

        public MainMenuLoadingState(
            IPlayerFactory playerFactory,
            IGameObjectFactory gameObjectFactory,
            IWindowService windowService,
            IStaticDataService staticDataService,
            IProjectStateMachineService projectStateMachineService,
            IAssetsAddressablesProvider assetsAddressablesProvider)
        {
            _playerFactory = playerFactory;
            _gameObjectFactory = gameObjectFactory;
            _windowService = windowService;
            _staticDataService = staticDataService;
            _projectStateMachineService = projectStateMachineService;
            _assetsAddressablesProvider = assetsAddressablesProvider;
        }

        public async void OnEnter()
        {
            await InstanceMainMenuScreen();
            await CreatePlayer();
            await CreateARSession();

            MoveToNextState();
        }

        private async Task InstanceMainMenuScreen()
        {
            var levelData = _staticDataService.GetLevelData<MainMenuLevelData>("MainMenuLevelData");

            await _windowService.OpenAndGet<MainMenuUI>(
                WindowID.MainMenu,
                levelData.ScreenSpawnPoint.Position,
                levelData.ScreenSpawnPoint.Rotation.ToQuaternion()
            );
        }

        private async Task CreatePlayer()
        {
            await _playerFactory.CreatePlayer();
        }

        private async Task CreateARSession()
        {
            await _gameObjectFactory.CreateInstance(AssetsAddressableConstants.AR_SESSION);
        }

        private void MoveToNextState()
        {
            _projectStateMachineService.SwitchState<MainMenuState>();
        }
    }
}