using System.Threading.Tasks;
using Data.Configurations.Level;
using Data.Contexts.Scene;
using Infrastructure.Providers.GlobalDataContainer;
using Infrastructure.Providers.SceneContainer;
using Infrastructure.Services.InteractorSetup;
using Infrastructure.Services.Player;
using Infrastructure.Services.ProjectManagement;
using Infrastructure.Services.Weapon;
using Infrastructure.Services.Window;
using Infrastructure.Services.XRSetup;
using UI.HandMenu;
using UI.InformationDesk;
using UnityEngine;

namespace Core.Project.Gameplay
{
    public class ConfigureState : IState, IEnterable
    {
        private readonly IGlobalContextProvider _globalContextProvider;
        private readonly ISceneContextProvider _sceneContextProvider;
        private readonly IWindowService _windowService;
        private readonly IXRSetupService _xrSetupService;
        private readonly IInteractorService _interactorService;
        private readonly IPlayerService _playerService;
        private readonly IWeaponService _weaponService;
        private readonly IProjectManagementService _projectManagementService;

        public ConfigureState(
            IGlobalContextProvider globalContextProvider,
            IProjectManagementService projectManagementService,
            ISceneContextProvider sceneContextProvider,
            IWindowService windowService,
            IXRSetupService xrSetupService,
            IInteractorService interactorService,
            IPlayerService playerService,
            IWeaponService weaponService
        )
        {
            _globalContextProvider = globalContextProvider;
            _sceneContextProvider = sceneContextProvider;
            _windowService = windowService;
            _xrSetupService = xrSetupService;
            _interactorService = interactorService;
            _playerService = playerService;
            _weaponService = weaponService;
            _projectManagementService = projectManagementService;
        }

        public async void OnEnter()
        {
            var levelData = _globalContextProvider.GlobalContext.LevelData;
            var sceneContextData = _sceneContextProvider.Get<GameplaySceneContextData>();

            ConfigurePlayer(levelData, sceneContextData.PlayerSpawnPoint);

            await InstantiateWeapon(sceneContextData.BowSpawnPoint);

            await InstantiateInfoScreen(sceneContextData.InfoScreenSpawnPoint);

            await InstantiateHandMenuScreen(_playerService.PlayerContainer.HandMenuSpawnPoint);

            MoveToGameplayState();
        }

        private void ConfigurePlayer(LevelData levelData, Transform playerSpawnPoint)
        {
            _xrSetupService.SetXRMode(levelData.XRMode);

            _interactorService.SetUpInteractor(HandType.Left, InteractorType.NearFar);
            _interactorService.SetUpInteractor(HandType.Right, InteractorType.Direct | InteractorType.Poke);

            _playerService.SetPlayerPositionAndRotation(playerSpawnPoint.position, playerSpawnPoint.rotation);
        }

        private async Task InstantiateWeapon(Transform spawnPoint) =>
            await _weaponService.InstantiateEquippedWeapon(spawnPoint.position, spawnPoint.rotation);

        private async Task InstantiateInfoScreen(Transform spawnPoint)
        {
            var infoScreen = await _windowService.OpenInWorldAndGet<InformationDeskUI>(WindowID.InformationDesk,
                spawnPoint.position, spawnPoint.rotation);
            
            infoScreen.SetTimeText("0.00");
            infoScreen.SetScoreText(string.Empty);
        }

        private async Task InstantiateHandMenuScreen(Transform spawnPoint)
        {
            var handMenuScreen = await _windowService.OpenInWorldAndGet<HandMenuUI>(WindowID.HandMenu,
                spawnPoint.position, spawnPoint.rotation, spawnPoint);

            handMenuScreen.OnExitButtonClicked += ExitInMainMenu;
        }

        private void ExitInMainMenu() => _projectManagementService.ChangeState<MoveToMainMenuState>();
        private void MoveToGameplayState() => _projectManagementService.ChangeState<WaitingToStartState>();
    }
}