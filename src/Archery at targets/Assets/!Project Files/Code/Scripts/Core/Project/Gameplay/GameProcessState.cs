using System;
using Core.Gameplay;
using Infrastructure.Providers.GlobalDataContainer;
using Infrastructure.Services.ProjectManagement;

namespace Core.Project.Gameplay
{
    public class GameProcessState : IState, IEnterable
    {
        private readonly IProjectManagementService _projectManagementService;
        private readonly IGlobalContextProvider _globalContextProvider;

        public GameProcessState(
            IProjectManagementService projectManagementService,
            IGlobalContextProvider globalContextProvider)
        {
            _projectManagementService = projectManagementService;
            _globalContextProvider = globalContextProvider;
        }

        public void OnEnter()
        {
            StartGameplay();
        }

        private async void StartGameplay()
        {
            var gameplayLevel = _globalContextProvider.GlobalContext.GameplayLevel;

            await gameplayLevel.StartGame();

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