using System.Threading.Tasks;
using Data.Contexts.Scene;
using Infrastructure.Providers.SceneContainer;
using Infrastructure.Services.ProjectManagement;
using Infrastructure.Services.Window;
using UI.Levels;
using UI.MainMenu;

namespace Core.Project.MainMenu
{
    public class InstantiateScreensState : IState, IEnterable
    {
        private readonly IProjectManagementService _projectManagementService;
        private readonly IWindowService _windowService;
        private readonly ISceneContextProvider _sceneContextProvider;

        public InstantiateScreensState(
            IProjectManagementService projectManagementService,
            IWindowService windowService,
            ISceneContextProvider sceneContextProvider)
        {
            _projectManagementService = projectManagementService;
            _windowService = windowService;
            _sceneContextProvider = sceneContextProvider;
        }

        public async void OnEnter()
        {
            await InstantiateMainMenuUI();

            await InstantiateLevelsUI();

            MoveToNextState();
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

        private void MoveToNextState() => _projectManagementService.ChangeState<ConfigureState>();
    }
}