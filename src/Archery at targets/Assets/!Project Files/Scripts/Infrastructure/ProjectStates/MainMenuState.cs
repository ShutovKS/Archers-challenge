using Data.Level;
using Infrastructure.Factories.GameObjects;
using Infrastructure.Services.ProjectStateMachine;
using Infrastructure.Services.StaticData;
using Infrastructure.Services.Window;
using Infrastructure.Services.XRSetup;
using JetBrains.Annotations;
using UI;

namespace Infrastructure.ProjectStates
{
    [UsedImplicitly]
    public class MainMenuState : IState, IEnterable, IExitable
    {
        private readonly IProjectStateMachineService _projectStateMachine;
        private readonly IGameObjectFactory _gameObjectFactory;
        private readonly IStaticDataService _staticDataService;
        private readonly IWindowService _windowService;
        private readonly IXRSetupService _xrSetupService;
        private MainMenuLevelData _levelData;
        private MainMenuUI _mainMenuUI;

        public MainMenuState(
            IProjectStateMachineService projectStateMachine,
            IStaticDataService staticDataService,
            IWindowService windowService,
            IXRSetupService xrSetupService)
        {
            _projectStateMachine = projectStateMachine;
            _staticDataService = staticDataService;
            _windowService = windowService;
            _xrSetupService = xrSetupService;
        }

        public void OnEnter()
        {
            _mainMenuUI = _windowService.Get<MainMenuUI>(WindowID.MainMenu);

            ConfigurePlayer();
            ShowMainMenu();
        }

        public void OnExit()
        {
            _windowService.Close(WindowID.MainMenu);
        }

        private void ConfigurePlayer()
        {
            // _playerFactory.Player.SetPositionAndRotation(_levelData.PlayerSpawnPoint);
        }

        private void ShowMainMenu()
        {
            _mainMenuUI.ClearButtons();
            _mainMenuUI.AddButton("VR режим", ShowVRGamesMenu);
            _mainMenuUI.AddButton("MR режим", ShowMrGamesMenu);
            _mainMenuUI.AddButton("Выход", ExitFromGame);
        }

        private void ShowVRGamesMenu()
        {
            _mainMenuUI.ClearButtons();
            // _mainMenuUI.AddButton("На количество попаданий", LoadGame<VRShootingPerNumberHitsState>);
            // _mainMenuUI.AddButton("На время", LoadGame<VRShootingForTimeState>);
            // _mainMenuUI.AddButton("Бесконечный режим", LoadGame<VRShootingInfiniteState>);   
            _mainMenuUI.AddButton("Назад", ShowMainMenu);
        }

        private void ShowMrGamesMenu()
        {
            _mainMenuUI.ClearButtons();
            // _mainMenuUI.AddButton("На количество попаданий", LoadGame<MRShootingPerNumberHitsState>);
            _mainMenuUI.AddButton("Назад", ShowMainMenu);
        }

        // private void LoadGame<T>() where T : IState => _projectStateMachine.SwitchState<LoadScenesState, string>(typeof(T).Name);

        private void ExitFromGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            UnityEngine.Application.Quit();
#endif
        }
    }
}