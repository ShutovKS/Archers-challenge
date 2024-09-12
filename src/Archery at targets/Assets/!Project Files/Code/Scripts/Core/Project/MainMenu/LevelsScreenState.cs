using Core.Project.Gameplay;
using Data.Configurations.Level;
using Infrastructure.Providers.GlobalDataContainer;
using Infrastructure.Providers.StaticData;
using Infrastructure.Services.ProjectManagement;
using Infrastructure.Services.Window;
using UI.Levels;

namespace Core.Project.MainMenu
{
    public class LevelsScreenState : IState, IEnterable, IExitable
    {
        private readonly IWindowService _windowService;
        private readonly IStaticDataProvider _staticDataProvider;
        private readonly IProjectManagementService _projectManagementService;
        private readonly IGlobalContextProvider _globalContextProvider;

        private LevelsUI _levelsUI;

        public LevelsScreenState(IWindowService windowService, IStaticDataProvider staticDataProvider,
            IProjectManagementService projectManagementService, IGlobalContextProvider globalContextProvider)
        {
            _windowService = windowService;
            _staticDataProvider = staticDataProvider;
            _projectManagementService = projectManagementService;
            _globalContextProvider = globalContextProvider;
        }

        public void OnEnter()
        {
            InitializeLevelsScreen();

            _levelsUI.Show();
        }

        private void InitializeLevelsScreen()
        {
            _levelsUI = _windowService.Get<LevelsUI>(WindowID.Levels);

            _levelsUI.OnBackClicked += ExitLevelsScreen;

            _levelsUI.OnItemClicked += StartLevel;
        }

        private void StartLevel(string levelId)
        {
            var levelData = _staticDataProvider.GetLevelData<LevelData>(levelId);
            _globalContextProvider.GlobalContext.LevelData = levelData;

            _projectManagementService.ChangeState<ExitMenuAndLaunchGameplay>();
        }

        private void ExitLevelsScreen() => _projectManagementService.ChangeState<MenuScreenState>();

        public void OnExit()
        {
            _levelsUI.Hide();
        }
    }
}