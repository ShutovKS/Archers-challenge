using System;
using Core.Gameplay;
using Infrastructure.Factories.GameplayLevels;
using Infrastructure.Providers.GlobalDataContainer;
using Infrastructure.Services.ProjectManagement;

namespace Core.Project.Gameplay
{
    public class GameProcessState : IState, IEnterable
    {
        private readonly IProjectManagementService _projectManagementService;
        private readonly IGameplayLevelsFactory _gameplayLevelsFactory;
        private readonly IGlobalContextProvider _globalContextProvider;

        public GameProcessState(
            IProjectManagementService projectManagementService,
            IGameplayLevelsFactory gameplayLevelsFactory,
            IGlobalContextProvider globalContextProvider)
        {
            _projectManagementService = projectManagementService;
            _gameplayLevelsFactory = gameplayLevelsFactory;
            _globalContextProvider = globalContextProvider;
        }

        public void OnEnter()
        {
            StartGameplay();
        }

        private async void StartGameplay()
        {
            var levelData = _globalContextProvider.GlobalContext.LevelData;
            var gameplayLevel = _gameplayLevelsFactory.Create(levelData.GameplayModeData.ModeType);

            await gameplayLevel.StartGame(levelData.GameplayModeData);
            gameplayLevel.OnGameFinished += GameFinished;
        }

        private void GameFinished(GameResult gameResult)
        {
            switch (gameResult)
            {
                case GameResult.Win:
                    
                case GameResult.Lose:
                    ExitInMainMenu();
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(gameResult), gameResult, null);
            }
        }

        private void ExitInMainMenu() => _projectManagementService.ChangeState<MoveToMainMenuState>();
    }
}