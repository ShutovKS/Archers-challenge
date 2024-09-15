using System.Threading.Tasks;
using Core.Project.MainMenu;
using Infrastructure.Providers.GlobalDataContainer;
using Infrastructure.Services.ProjectManagement;
using Infrastructure.Services.SceneLoader;
using Infrastructure.Services.Window;

namespace Core.Project.Gameplay
{
    public class MoveToMainMenuState : IState, IEnterable
    {
        private readonly IProjectManagementService _projectManagementService;
        private readonly ISceneLoaderService _sceneLoaderService;
        private readonly IGlobalContextProvider _globalContextProvider;
        private readonly IWindowService _windowService;

        public MoveToMainMenuState(IProjectManagementService projectManagementService,
            ISceneLoaderService sceneLoaderService, IGlobalContextProvider globalContextProvider,
            IWindowService windowService)
        {
            _projectManagementService = projectManagementService;
            _sceneLoaderService = sceneLoaderService;
            _globalContextProvider = globalContextProvider;
            _windowService = windowService;
        }

        public async void OnEnter()
        {
            var levelData = _globalContextProvider.GlobalContext.LevelData;

            CloseScreens();

            await DestroyLocation(levelData.LocationScenePath);

            MoveToNextState();
        }

        private async Task DestroyLocation(string levelDataLocationScenePath) =>
            await _sceneLoaderService.UnloadSceneAsync(levelDataLocationScenePath);

        private void CloseScreens()
        {
            _windowService.Close(WindowID.HandMenu);
            _windowService.Close(WindowID.InformationDesk);
        }

        private void MoveToNextState() => _projectManagementService.ChangeState<MainMenuState>();
    }
}