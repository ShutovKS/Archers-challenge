using System.Threading.Tasks;
using Data.Contexts.Scene;
using Infrastructure.Providers.SceneContainer;
using Infrastructure.Services.Player;
using Infrastructure.Services.ProjectManagement;
using Infrastructure.Services.Weapon;
using Infrastructure.Services.Window;
using UI.HandMenu;
using UI.InformationDesk;
using UnityEngine;

namespace Core.Project.Gameplay
{
    public class ObjectsBootState : IState, IEnterable
    {
        private readonly IWindowService _windowService;
        private readonly IPlayerService _playerService;
        private readonly IWeaponService _weaponService;
        private readonly ISceneContextProvider _sceneContextProvider;
        private readonly IProjectManagementService _projectManagementService;

        public ObjectsBootState(
            IWindowService windowService,
            IPlayerService playerService,
            IWeaponService weaponService,
            ISceneContextProvider sceneContextProvider,
            IProjectManagementService projectManagementService
        )
        {
            _windowService = windowService;
            _playerService = playerService;
            _weaponService = weaponService;
            _sceneContextProvider = sceneContextProvider;
            _projectManagementService = projectManagementService;
        }

        public async void OnEnter()
        {
            var sceneContextData = _sceneContextProvider.Get<GameplaySceneContextData>();

            await InstantiateWeapon(sceneContextData.BowSpawnPoint);

            await InstantiateInfoScreen(sceneContextData.InfoScreenSpawnPoint);

            await InstantiateHandMenuScreen(_playerService.PlayerContainer.HandMenuSpawnPoint);
            
            MoveToNextState();
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
        
        private void MoveToNextState() => _projectManagementService.ChangeState<ConfigureState>();
    }
}