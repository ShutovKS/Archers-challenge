#region

using System;
using Core.Gameplay;
using Core.Project.MainMenu;
using Data.Configurations.Level;
using Infrastructure.Services.GameSetup;
using Infrastructure.Services.Player;
using Infrastructure.Services.ProjectManagement;
using Infrastructure.Services.Weapon;
using UI.HandMenu;
using UnityEngine;

#endregion

namespace Core.Project.Gameplay
{
    public class GameplayState : IState, IEnterableWithArg<LevelData>, IExitable
    {
        private readonly IProjectManagementService _projectManagementService;
        private readonly IGameplaySetupService _gameplaySetupService;
        private readonly IWeaponService _weaponService;
        private readonly IPlayerService _playerService;
        private IGameplayLevel _gameplayLevel;

        public GameplayState(
            IProjectManagementService projectManagementService,
            IGameplaySetupService gameplaySetupService,
            IWeaponService weaponService,
            IPlayerService playerService
            )
        {
            _projectManagementService = projectManagementService;
            _gameplaySetupService = gameplaySetupService;
            _weaponService = weaponService;
            _playerService = playerService;
        }

        public async void OnEnter(LevelData levelData)
        {
            await _gameplaySetupService.SetupGameplayAsync(levelData);
            SetupEventHandlers();
        }

        private void SetupEventHandlers()
        {
            _playerService.PlayerContainer.HandMenuUI.OnExitButtonClicked += ExitInMainMenu;
            _weaponService.CurrentWeapon.OnSelected += OnWeaponSelected;
        }

        private void OnWeaponSelected(bool isSelected)
        {
            if (!isSelected) return;
            
            _weaponService.CurrentWeapon.OnSelected -= OnWeaponSelected;

            LaunchGameplay();
        }

        private async void LaunchGameplay()
        {
            _gameplayLevel = await _gameplaySetupService.LaunchGameplayAsync();
            _gameplayLevel.OnGameFinished += GameFinished;
        }

        private void GameFinished(GameResult gameResult)
        {
            switch (gameResult)
            {
                case GameResult.Win:
                    ExitInMainMenu();
                    break;
                case GameResult.Lose:
                    ExitInMainMenu();
                    break;
                case GameResult.Error:
                    ExitInMainMenu();
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(gameResult), gameResult, null);
            }
        }

        private void ExitInMainMenu()
        {
            _projectManagementService.ChangeState<MainMenuState>();
        }

        public async void OnExit()
        {
            _playerService.PlayerContainer.HandMenuUI.OnExitButtonClicked -= ExitInMainMenu;
            _gameplayLevel?.StopGame();
            await _gameplaySetupService.CleanupGameplayAsync();
        }
    }
}