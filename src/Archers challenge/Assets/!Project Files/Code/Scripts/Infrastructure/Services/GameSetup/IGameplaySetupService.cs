using System.Threading.Tasks;
using Core.Gameplay;
using Data.Configurations.Level;
using Data.Contexts.Scene;
using Infrastructure.Factories.GameplayLevels;
using Infrastructure.Providers.SceneContainer;
using Infrastructure.Services.InteractorSetup;
using Infrastructure.Services.Player;
using Infrastructure.Services.SceneLoader;
using Infrastructure.Services.Weapon;
using Infrastructure.Services.Window;
using Infrastructure.Services.XRSetup;
using UI.InformationDesk;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Infrastructure.Services.GameSetup
{
    public interface IGameplaySetupService
    {
        Task SetupGameplayAsync(LevelData levelData);
        Task<IGameplayLevel> LaunchGameplayAsync();
        Task CleanupGameplayAsync();
    }

    public class GameplaySetupService : IGameplaySetupService
    {
        private readonly ISceneLoaderService _sceneLoaderService;
        private readonly ISceneContextProvider _sceneContextProvider;
        private readonly IWeaponService _weaponService;
        private readonly IWindowService _windowService;
        private readonly IPlayerService _playerService;
        private readonly IXRSetupService _xrSetupService;
        private readonly IInteractorService _interactorService;
        private readonly IGameplayLevelsFactory _gameplayLevelsFactory;

        private GameplaySceneContextData _sceneContextData;
        private IGameplayLevel _gameplayLevel;
        private LevelData _levelData;
        private SceneInstance _locationSceneInstance;

        public GameplaySetupService(ISceneLoaderService sceneLoaderService, ISceneContextProvider sceneContextProvider,
            IWeaponService weaponService, IWindowService windowService, IPlayerService playerService,
            IXRSetupService xrSetupService,
            IInteractorService interactorService, IGameplayLevelsFactory gameplayLevelsFactory)
        {
            _sceneLoaderService = sceneLoaderService;
            _sceneContextProvider = sceneContextProvider;
            _weaponService = weaponService;
            _windowService = windowService;
            _playerService = playerService;
            _xrSetupService = xrSetupService;
            _interactorService = interactorService;
            _gameplayLevelsFactory = gameplayLevelsFactory;
        }

        public async Task SetupGameplayAsync(LevelData levelData)
        {
            _levelData = levelData;

            await ConfigurePlayerXRSettings();
            await LoadLocationAsync();

            _sceneContextData = _sceneContextProvider.Get<GameplaySceneContextData>();

            await InstantiateWeapon();
            await OpenScreens();
            await ConfigurePlayer();
            await CreateGameplayLevel();
        }

        #region Setup Gameplay

        private async Task LoadLocationAsync() => _locationSceneInstance =
            await _sceneLoaderService.LoadSceneAsync(_levelData.LocationScenePath, LoadSceneMode.Additive);

        private async Task InstantiateWeapon() => await _weaponService.InstantiateEquippedWeapon(
            _sceneContextData.BowSpawnPoint.position,
            _sceneContextData.BowSpawnPoint.rotation,
            _levelData.IsGravityEnabled,
            _sceneContextData.BowForce
        );

        private async Task OpenScreens()
        {
            await InstantiateInfoScreen();
            await InstantiateHandMenuScreen();
        }

        private async Task InstantiateInfoScreen()
        {
            var infoScreen = await _windowService.OpenInWorldAndGet<InformationDeskUI>(
                WindowID.InformationDesk,
                _sceneContextData.InfoScreenSpawnPoint.position,
                _sceneContextData.InfoScreenSpawnPoint.rotation
            );

            infoScreen.SetTimeText("0.00");
            infoScreen.SetScoreText(string.Empty);
        }

        private async Task InstantiateHandMenuScreen()
        {
            var spawnPoint = _playerService.PlayerContainer.HandMenuSpawnPoint;
            await _windowService.OpenInWorld(WindowID.HandMenu, spawnPoint.position, spawnPoint.rotation, spawnPoint);
        }

        private Task ConfigurePlayerXRSettings()
        {
            _xrSetupService.SetXRMode(_levelData.XRMode);

            _interactorService.SetUpInteractor(HandType.Left, InteractorType.Ray);
            _interactorService.SetUpInteractor(HandType.Right, InteractorType.Direct | InteractorType.Poke);

            return Task.CompletedTask;
        }

        private Task ConfigurePlayer()
        {
            var playerSpawnPoint = _sceneContextData.PlayerSpawnPoint;
            _playerService.SetPlayerPositionAndRotation(playerSpawnPoint.position, playerSpawnPoint.rotation);

            return Task.CompletedTask;
        }

        private async Task CreateGameplayLevel()
        {
            _gameplayLevel = _gameplayLevelsFactory.Create(_levelData.GameplayModeData.ModeType);

            await _gameplayLevel.PrepareGame(_levelData.GameplayModeData);
        }

        #endregion

        public async Task<IGameplayLevel> LaunchGameplayAsync()
        {
            await _gameplayLevel.StartGame();
            return _gameplayLevel;
        }

        #region Launch Gameplay

        #endregion

        public async Task CleanupGameplayAsync()
        {
            await CleanupGameplayLevel();
            await CloseScreens();
            await DestroyLocation();
            await DestroyWeapon();
        }

        #region Cleanup Gameplay

        private Task CleanupGameplayLevel()
        {
            _gameplayLevel.CleanUp();

            return Task.CompletedTask;
        }

        private async Task DestroyLocation() => await _sceneLoaderService.UnloadSceneAsync(_locationSceneInstance);

        private Task CloseScreens()
        {
            _windowService.Close(WindowID.HandMenu);
            _windowService.Close(WindowID.InformationDesk);
            return Task.CompletedTask;
        }

        private Task DestroyWeapon()
        {
            _weaponService.DestroyWeapon();
            return Task.CompletedTask;
        }

        #endregion
    }
}