using System.Threading.Tasks;
using Infrastructure.Providers.GlobalDataContainer;
using Infrastructure.Services.ProjectManagement;
using Infrastructure.Services.SceneLoader;
using UnityEngine.SceneManagement;

namespace Core.Project.Gameplay
{
    public class LocationBootState : IState, IEnterable
    {
        private readonly IGlobalContextProvider _globalContextProvider;
        private readonly ISceneLoaderService _sceneLoaderService;
        private readonly IProjectManagementService _projectManagementService;

        public LocationBootState(IGlobalContextProvider globalContextProvider, ISceneLoaderService sceneLoaderService,
            IProjectManagementService projectManagementService)
        {
            _globalContextProvider = globalContextProvider;
            _sceneLoaderService = sceneLoaderService;
            _projectManagementService = projectManagementService;
        }

        public async void OnEnter()
        {
            var levelData = _globalContextProvider.GlobalContext.LevelData;

            await InstantiateLocation(levelData.LocationScenePath);

            MoveToNextState();
        }

        private async Task InstantiateLocation(string locationScenePath) =>
            await _sceneLoaderService.LoadSceneAsync(locationScenePath, LoadSceneMode.Additive);

        private void MoveToNextState() => _projectManagementService.ChangeState<ObjectsBootState>();
    }
}