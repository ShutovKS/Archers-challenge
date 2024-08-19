using System.Threading.Tasks;
using Data.GameplayConfigure;
using Data.Level;
using Extension;
using Infrastructure.Factories.Player;
using Infrastructure.ProjectStateMachine;
using Infrastructure.Services.InteractorSetup;
using Infrastructure.Services.StaticData;
using Infrastructure.Services.Window;
using Infrastructure.Services.XRSetup;
using Infrastructure.Services.XRSetup.TrackingMode;
using JetBrains.Annotations;
using UI;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;

namespace Infrastructure.ProjectStates
{
    [UsedImplicitly]
    public class MainMenuState : IState, IEnterable, IExitable
    {
        private readonly IProjectStateMachine _projectStateMachine;
        private readonly IStaticDataService _staticDataService;
        private readonly IWindowService _windowService;
        private readonly IXRSetupService _xrSetupService;
        private readonly IInteractorSetupService _interactorSetupService;
        private readonly IPlayerFactory _playerFactory;

        private MainMenuLevelData _levelData;
        private MainMenuUI _mainMenuUI;
        private GameplayConfigure _gameplayConfigure;

        public MainMenuState(
            IProjectStateMachine projectStateMachine,
            IStaticDataService staticDataService,
            IWindowService windowService,
            IXRSetupService xrSetupService,
            IInteractorSetupService interactorSetupService,
            IPlayerFactory playerFactory)
        {
            _projectStateMachine = projectStateMachine;
            _staticDataService = staticDataService;
            _windowService = windowService;
            _xrSetupService = xrSetupService;
            _interactorSetupService = interactorSetupService;
            _playerFactory = playerFactory;
        }

        public async void OnEnter()
        {
            try
            {
                _levelData = _staticDataService.GetLevelData<MainMenuLevelData>("MainMenuLevelData");
                _gameplayConfigure = new GameplayConfigure();

                await SetupMainMenuScreen();
                ConfigurePlayer();
                InitializeMainMenu();
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Error during MainMenuState OnEnter: {ex.Message}");
            }
        }

        public void OnExit()
        {
            try
            {
                _windowService.Close(WindowID.MainMenu);
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Error during MainMenuState OnExit: {ex.Message}");
            }
        }

        private async Task SetupMainMenuScreen()
        {
            try
            {
                _mainMenuUI = await _windowService.OpenAndGet<MainMenuUI>(
                    WindowID.MainMenu,
                    _levelData.ScreenSpawnPoint.Position,
                    _levelData.ScreenSpawnPoint.Rotation.ToQuaternion()
                );
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Error during SetupMainMenuScreen: {ex.Message}");
                throw;
            }
        }

        private void ConfigurePlayer()
        {
            try
            {
                _xrSetupService.SetXRMode(XRMode.VR);
                _playerFactory.Player.SetPositionAndRotation(_levelData.PlayerSpawnPoint);
                _interactorSetupService.SetInteractor(InteractorType.Ray);
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Error during ConfigurePlayer: {ex.Message}");
                throw;
            }
        }

        private void InitializeMainMenu()
        {
            try
            {
                _mainMenuUI.ClearButtons();
                AddMainMenuButtons();

                _gameplayConfigure.GameplayType = GameplayType.None;
                _gameplayConfigure.XRMode = XRMode.None;
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Error during InitializeMainMenu: {ex.Message}");
                throw;
            }
        }

        private void AddMainMenuButtons()
        {
            _mainMenuUI.AddButton("VR режим", ShowVRGamesMenu);
            _mainMenuUI.AddButton("MR режим", ShowMrGamesMenu);
            _mainMenuUI.AddButton("Выход", ExitFromGame);
        }

        private void ShowVRGamesMenu()
        {
            ConfigureGameplayMenu(XRMode.VR);
            _gameplayConfigure.XRTrackingMode = new NoneTrackingMode();
        }

        private void ShowMrGamesMenu()
        {
            ConfigureGameplayMenu(XRMode.MR);
            _gameplayConfigure.XRTrackingMode = new PlaneAndBoundingBoxTrackingMode
            {
                PlanePrefab = null,
                PlaneDetectionMode = PlaneDetectionMode.Horizontal | PlaneDetectionMode.Vertical,
                BoundingBoxPrefab = null
            };
        }

        private void ConfigureGameplayMenu(XRMode mode)
        {
            try
            {
                _mainMenuUI.ClearButtons();
                AddGameplayMenuButtons();
                _mainMenuUI.AddButton("Назад", InitializeMainMenu);

                _gameplayConfigure.XRMode = mode;
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Error during ConfigureGameplayMenu: {ex.Message}");
                throw;
            }
        }

        private void AddGameplayMenuButtons()
        {
            _mainMenuUI.AddButton("На количество попаданий", () => LoadGame(GameplayType.PerNumberHitsState));
            _mainMenuUI.AddButton("На время", () => LoadGame(GameplayType.ForTimeState));
            _mainMenuUI.AddButton("Бесконечный режим", () => LoadGame(GameplayType.InfiniteState));
        }

        private void LoadGame(GameplayType gameplayType)
        {
            try
            {
                _gameplayConfigure.GameplayType = gameplayType;
                _projectStateMachine.SwitchState<GameplayState, GameplayType>(gameplayType);
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Error during LoadGame: {ex.Message}");
                throw;
            }
        }

        private void ExitFromGame()
        {
            try
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                UnityEngine.Application.Quit();
#endif
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Error during ExitFromGame: {ex.Message}");
                throw;
            }
        }
    }
}