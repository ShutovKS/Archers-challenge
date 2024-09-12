#region

using System;
using System.Threading.Tasks;
using Core.Project.MainMenu;
using Data.Level;
using Features.PositionsContainer;
using Features.Weapon;
using Infrastructure.Factories.GameplayLevels;
using Infrastructure.Services.InteractorSetup;
using Infrastructure.Services.ProjectManagement;
using Infrastructure.Services.SceneLoader;
using Infrastructure.Services.XRSetup;
using JetBrains.Annotations;
using Logics.GameplayLevels;
using UI.InformationDesk;
using UnityEngine;
using UnityEngine.SceneManagement;

#endregion

namespace Logics.Project
{
    public class GameplayState : IState, IEnterableWithArg<GameplayLevelData>, IExitable
    {
        private readonly IProjectManagementService _projectManagementService;
        private readonly ISceneLoaderService _sceneLoaderService;
        private readonly IXRSetupService _xrSetupService;
        private readonly IInteractorService _interactorService;
        private readonly IGameplayLevelsFactory _gameplayLevelsFactory;

        private GameplayLevelData _levelData;
        private IGameplayLevel _gameplayLevel;

        private InformationDeskUI _infoScreen;
        private PositionsContainer _positionsContainer;
        private Bow _bow;

        private int _targetCount;

        public GameplayState(
            IProjectManagementService projectManagementService,
            ISceneLoaderService sceneLoaderService,
            IXRSetupService xrSetupService,
            IInteractorService interactorService,
            IGameplayLevelsFactory gameplayLevelsFactory
        )
        {
            _projectManagementService = projectManagementService;
            _sceneLoaderService = sceneLoaderService;
            _xrSetupService = xrSetupService;
            _interactorService = interactorService;
            _gameplayLevelsFactory = gameplayLevelsFactory;
        }

        public async void OnEnter(GameplayLevelData levelData)
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

            _interactorService.SetUpInteractor(HandType.Left, InteractorType.NearFar);
            _interactorService.SetUpInteractor(HandType.Right, InteractorType.Direct | InteractorType.Poke);
        }

        private async Task StartGameplay()
        {
            _gameplayLevel = _gameplayLevelsFactory.Create(_levelData.GameplayModeData.Mode);

            await _gameplayLevel.StartGame(_levelData.GameplayModeData);

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
                    Win();
                    break;
                case GameResult.Lose:
                    Lose();
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(gameResult), gameResult, null);
            }
        }

        private void Win()
        {
            Debug.Log("Win");

            ExitInMainMenu();
        }

        private void Lose()
        {
            Debug.Log("Lose");

            ExitInMainMenu();
        }

        public void OnExit()
        {
            DestroyLocation();
        }

        private void ExitInMainMenu()
        {
            _projectManagementService.ChangeState<MainMenuState>();
        }

        private void DestroyLocation()
        {
            _sceneLoaderService.UnloadSceneAsync(_levelData.LocationScenePath);
        }
    }
}