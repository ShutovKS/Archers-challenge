using Core.Project.Gameplay;
using Data.Configurations.Level;
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

        private LevelsUI _levelsUI;

        public LevelsScreenState(IWindowService windowService, IStaticDataProvider staticDataProvider,
            IProjectManagementService projectManagementService)
        {
            _windowService = windowService;
            _staticDataProvider = staticDataProvider;
            _projectManagementService = projectManagementService;
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
            var levelData = _staticDataProvider.GetLevelData<GameplayLevelData>(levelId);

            _projectManagementService.ChangeState<GameplayState, GameplayLevelData>(levelData);
        }

        private void ExitLevelsScreen()
        {
            _projectManagementService.ChangeState<MenuScreenState>();
        }

        public void OnExit()
        {
            _levelsUI.Hide();
        }
    }
}