using System.Linq;
using System.Threading.Tasks;
using Data.Configurations.Level;
using Data.Contexts.Scene;
using Infrastructure.Providers.SceneContainer;
using Infrastructure.Providers.StaticData;
using Infrastructure.Services.InteractorSetup;
using Infrastructure.Services.Player;
using Infrastructure.Services.SceneLoader;
using Infrastructure.Services.Window;
using Infrastructure.Services.XRSetup;
using UI.Levels;
using UI.MainMenu;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Infrastructure.Services.GameSetup
{
    public interface IMainMenuSetupService
    {
        Task SetupMainMenuAsync();
        Task CleanupMainMenuAsync();
    }

    public class MainMenuSetupService : IMainMenuSetupService
    {
        private readonly ISceneLoaderService _sceneLoaderService;
        private readonly IStaticDataProvider _staticDataProvider;
        private readonly IWindowService _windowService;
        private readonly ISceneContextProvider _sceneContextProvider;
        private readonly IPlayerService _playerService;
        private readonly IXRSetupService _xrSetupService;
        private readonly IInteractorService _interactorService;

        private MainMenuSceneContextData _sceneContextData;
        private LevelData _levelData;
        private SceneInstance _sceneInstance;

        public MainMenuSetupService(ISceneLoaderService sceneLoaderService, IStaticDataProvider staticDataProvider,
            IWindowService windowService, ISceneContextProvider sceneContextProvider, IPlayerService playerService,
            IXRSetupService xrSetupService, IInteractorService interactorService)
        {
            _sceneLoaderService = sceneLoaderService;
            _staticDataProvider = staticDataProvider;
            _windowService = windowService;
            _sceneContextProvider = sceneContextProvider;
            _playerService = playerService;
            _xrSetupService = xrSetupService;
            _interactorService = interactorService;
        }

        public async Task SetupMainMenuAsync()
        {
            _levelData = _staticDataProvider.GetLevelData<LevelData>("MainMenu");

            await LoadMainMenuScene();

            _sceneContextData = _sceneContextProvider.Get<MainMenuSceneContextData>();

            await OpenScreens();
            await ConfigurePlayer();
            await ConfigureLevelsUI();
        }

        #region Setup MainMenu

        private async Task LoadMainMenuScene() => _sceneInstance =
            await _sceneLoaderService.LoadSceneAsync(_levelData.LocationScenePath, LoadSceneMode.Additive);

        private async Task OpenScreens()
        {
            await InstantiateMainMenuUI();
            await InstantiateLevelsUI();
        }

        private async Task InstantiateMainMenuUI()
        {
            var mainMenuUI = await _windowService.OpenInWorldAndGet<MainMenuUI>(WindowID.MainMenu,
                _sceneContextData.MainMenuScreenSpawnPoint.position,
                _sceneContextData.MainMenuScreenSpawnPoint.rotation);

            mainMenuUI.Hide();
        }

        private async Task InstantiateLevelsUI()
        {
            var levelsUI = await _windowService.OpenInWorldAndGet<LevelsUI>(WindowID.Levels,
                _sceneContextData.LevelsScreenSpawnPoint.position,
                _sceneContextData.LevelsScreenSpawnPoint.rotation);

            levelsUI.Hide();
        }

        private Task ConfigurePlayer()
        {
            _xrSetupService.SetXRMode(XRMode.VR);
            _interactorService.SetUpInteractor(HandType.Left, InteractorType.Ray);
            _interactorService.SetUpInteractor(HandType.Right, InteractorType.Ray);

            _playerService.SetPlayerPositionAndRotation(
                _sceneContextData.PlayerSpawnPoint.position,
                _sceneContextData.PlayerSpawnPoint.rotation);

            return Task.CompletedTask;
        }

        private Task ConfigureLevelsUI()
        {
            var levelsUI = _windowService.Get<LevelsUI>(WindowID.Levels);

            var gameplayLevelDatas = _staticDataProvider.GetLevelData<LevelData>();
            gameplayLevelDatas = gameplayLevelDatas.Where(x => x.LevelIndex > 0).ToArray();

            levelsUI.SetItems(gameplayLevelDatas);

            return Task.CompletedTask;
        }

        #endregion

        public async Task CleanupMainMenuAsync()
        {
            await UnloadLocation();
            await CloseScreens();
        }

        #region Cleanup MainMenu

        private async Task UnloadLocation() => await _sceneLoaderService.UnloadSceneAsync(_sceneInstance);

        private Task CloseScreens()
        {
            _windowService.Close(WindowID.MainMenu);
            _windowService.Close(WindowID.Levels);

            return Task.CompletedTask;
        }

        #endregion
    }
}