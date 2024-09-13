using System.Threading.Tasks;
using Data.Configurations.Level;
using Infrastructure.Providers.StaticData;
using Infrastructure.Services.ProjectManagement;
using Infrastructure.Services.SceneLoader;
using UnityEngine.SceneManagement;

namespace Core.Project.MainMenu
{
    public class LocationBootState : IState, IEnterable
    {
        private readonly IProjectManagementService _projectManagementService;
        private readonly IStaticDataProvider _staticDataProvider;
        private readonly ISceneLoaderService _sceneLoaderService;

        public LocationBootState(
            IProjectManagementService projectManagementService,
            IStaticDataProvider staticDataProvider,
            ISceneLoaderService sceneLoaderService
        )
        {
            _projectManagementService = projectManagementService;
            _staticDataProvider = staticDataProvider;
            _sceneLoaderService = sceneLoaderService;
        }

        public async void OnEnter()
        {
            await LoadMainMenuScene();

            MoveToNextState();
        }

        private async Task LoadMainMenuScene()
        {
            var levelData = _staticDataProvider.GetLevelData<LevelData>("MainMenu");
            await _sceneLoaderService.LoadSceneAsync(levelData.LocationScenePath, LoadSceneMode.Additive);
        }

        private void MoveToNextState() => _projectManagementService.ChangeState<InstantiateScreensState>();
    }
}