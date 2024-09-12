using System.Threading.Tasks;
using Data.Level;
using Data.SceneContext;
using Infrastructure.Services.ProjectManagement;
using Infrastructure.Services.SceneContainer;
using Infrastructure.Services.SceneLoader;
using Infrastructure.Services.StaticData;
using Infrastructure.Services.Window;
using UI.Levels;
using UI.MainMenu;
using UnityEngine.SceneManagement;

namespace Core.Project.MainMenu
{
    public class BootState : IState, IEnterable
    {
        private readonly IProjectManagementService _projectManagementService;
        private readonly IStaticDataService _staticDataService;
        private readonly IWindowService _windowService;
        private readonly ISceneLoaderService _sceneLoaderService;
        private readonly ISceneContextProvider _sceneContextProvider;

        public BootState(
            IProjectManagementService projectManagementService,
            IStaticDataService staticDataService,
            IWindowService windowService,
            ISceneLoaderService sceneLoaderService,
            ISceneContextProvider sceneContextProvider)
        {
            _projectManagementService = projectManagementService;
            _staticDataService = staticDataService;
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
            var levelData = _staticDataService.GetLevelData<LevelData>("MainMenu");
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