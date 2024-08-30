using System;
using System.Threading.Tasks;
using Data.Level;
using Features.PositionsContainer;
using Features.Weapon;
using Infrastructure.GameplayLevels;
using Infrastructure.Services.InteractorSetup;
using Infrastructure.Services.ProjectManagement;
using Infrastructure.Services.SceneLoader;
using Infrastructure.Services.XRSetup;
using JetBrains.Annotations;
using UI.InformationDesk;
using UnityEngine.SceneManagement;

namespace Infrastructure.ProjectStates
{
    [UsedImplicitly]
    public class GameplayState : IState, IEnterableWithArg<LevelData>, IExitable
    {
        private readonly IProjectManagementService _projectManagementService;
        private readonly ISceneLoaderService _sceneLoaderService;
        private readonly IXRSetupService _xrSetupService;
        private readonly IInteractorService _interactorService;
        private readonly IGameplayLevel _gameplayLevel;

        private LevelData _levelData;

        private InformationDeskUI _infoScreen;
        private PositionsContainer _positionsContainer;
        private Bow _bow;

        private int _targetCount;

        public GameplayState(
            IProjectManagementService projectManagementService,
            ISceneLoaderService sceneLoaderService,
            IXRSetupService xrSetupService,
            IInteractorService interactorService,
            IGameplayLevel gameplayLevel
        )
        {
            _projectManagementService = projectManagementService;
            _sceneLoaderService = sceneLoaderService;
            _xrSetupService = xrSetupService;
            _interactorService = interactorService;
            _gameplayLevel = gameplayLevel;
        }

        public async void OnEnter(LevelData levelData)
        {
            _levelData = levelData;

            await InstantiateLocation();

            ConfigurePlayer();

            await StartGameplay();
        }

        private async Task InstantiateLocation()
        {
            await _sceneLoaderService.LoadSceneAsync(_levelData.LocationScenePath, LoadSceneMode.Additive);
        }

        private void ConfigurePlayer()
        {
            _xrSetupService.SetXRMode(_levelData.XRMode);

            _interactorService.SetUpInteractorForHand(HandType.Left, InteractorType.NearFar);
            _interactorService.SetUpInteractorForHand(HandType.Right, InteractorType.Direct | InteractorType.Poke);
        }

        private async Task StartGameplay()
        {
            await _gameplayLevel.StartGame();

            _gameplayLevel.OnGameFinished += GameFinished;
        }

        private void GameFinished(GameResult gameResult)
        {
            switch (gameResult)
            {
                case GameResult.Exit:
                    ExitInMainMenu();
                    break;
                case GameResult.Win:
                case GameResult.Lose:
                default: throw new ArgumentOutOfRangeException(nameof(gameResult), gameResult, null);
            }
        }

        private void ExitInMainMenu()
        {
            _projectManagementService.SwitchState<MainMenuState>();
        }

        public void OnExit()
        {
            DestroyLocation();
        }

        private void DestroyLocation()
        {
            _sceneLoaderService.UnloadSceneAsync(_levelData.LocationScenePath);
        }
    }
}