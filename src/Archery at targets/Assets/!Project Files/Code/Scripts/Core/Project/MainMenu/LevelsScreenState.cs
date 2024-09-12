using Data.Level;
using Infrastructure.Services.ProjectManagement;
using Infrastructure.Services.StaticData;
using Infrastructure.Services.Window;
using Logics.Project;
using UI.Levels;

namespace Core.Project.MainMenu
{
    public class LevelsScreenState : IState, IEnterable, IExitable
    {
        private readonly IWindowService _windowService;
        private readonly IStaticDataService _staticDataService;
        private readonly IProjectManagementService _projectManagementService;

        private LevelsUI _levelsUI;

        public LevelsScreenState(IWindowService windowService, IStaticDataService staticDataService,
            IProjectManagementService projectManagementService)
        {
            _windowService = windowService;
            _staticDataService = staticDataService;
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
            var levelData = _staticDataService.GetLevelData<GameplayLevelData>(levelId);

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