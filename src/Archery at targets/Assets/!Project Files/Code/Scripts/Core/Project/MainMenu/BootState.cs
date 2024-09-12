using System.Threading.Tasks;
using Data.Configurations.Level;
using Data.Contexts.Scene;
using Infrastructure.Providers.SceneContainer;
using Infrastructure.Providers.StaticData;
using Infrastructure.Services.ProjectManagement;
using Infrastructure.Services.SceneLoader;
using Infrastructure.Services.Window;
using UI.Levels;
using UI.MainMenu;
using UnityEngine.SceneManagement;

namespace Core.Project.MainMenu
{
    public class BootState : IState, IEnterable
    {
        private readonly IProjectManagementService _projectManagementService;
        private readonly IStaticDataProvider _staticDataProvider;
        private readonly IWindowService _windowService;
        private readonly ISceneLoaderService _sceneLoaderService;
        private readonly ISceneContextProvider _sceneContextProvider;

        public BootState(
            IProjectManagementService projectManagementService,
            IStaticDataProvider staticDataProvider,
            IWindowService windowService,
            ISceneLoaderService sceneLoaderService,
            ISceneContextProvider sceneContextProvider)
        {
            _projectManagementService = projectManagementService;
            _staticDataProvider = staticDataProvider;
            _windowService = windowService;
            _sceneLoaderService = sceneLoaderService;
            _sceneContextProvider = sceneContextProvider;
        }

        public async void OnEnter()
        {
            await LoadMainMenuScene();
            await InstantiateMainMenuUI();
            await InstantiateLevelsUI();
            MoveToConfigureState();
        }

        private async Task LoadMainMenuScene()
        {
            var levelData = _staticDataProvider.GetLevelData<LevelData>("MainMenu");
            await _sceneLoaderService.LoadSceneAsync(levelData.LocationScenePath, LoadSceneMode.Additive);
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

            levelsUI.Hide();
        }

        private void MoveToConfigureState()
        {
            _projectManagementService.ChangeState<ConfigureState>();
        }
    }
}