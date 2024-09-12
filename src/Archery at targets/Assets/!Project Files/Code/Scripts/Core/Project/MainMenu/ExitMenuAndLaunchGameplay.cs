using System.Threading.Tasks;
using Core.Project.Gameplay;
using Data.Configurations.Level;
using Infrastructure.Providers.StaticData;
using Infrastructure.Services.ProjectManagement;
using Infrastructure.Services.SceneLoader;
using Infrastructure.Services.Window;

namespace Core.Project.MainMenu
{
    public class ExitMenuAndLaunchGameplay : IState, IEnterable
    {
        private readonly IProjectManagementService _projectManagementService;
        private readonly ISceneLoaderService _sceneLoaderService;
        private readonly IStaticDataProvider _staticDataProvider;
        private readonly IWindowService _windowService;

        public ExitMenuAndLaunchGameplay(
            IProjectManagementService projectManagementService,
            ISceneLoaderService sceneLoaderService,
            IStaticDataProvider staticDataProvider,
            IWindowService windowService
        )
        {
            _projectManagementService = projectManagementService;
            _sceneLoaderService = sceneLoaderService;
            _staticDataProvider = staticDataProvider;
            _windowService = windowService;
        }

        public async void OnEnter()
        {
            await UnloadLocation();
            ClosesScreens();
            LaunchGameplay();
        }

        private async Task UnloadLocation()
        {
            var levelData = _staticDataProvider.GetLevelData<LevelData>("MainMenu");
            await _sceneLoaderService.UnloadSceneAsync(levelData.LocationScenePath);
        }

        private void ClosesScreens() => _windowService.Close(WindowID.MainMenu);

        private void LaunchGameplay() => _projectManagementService.ChangeState<GameplayState>();
    }
}