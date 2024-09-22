#region

using System;
using Core.Gameplay;
using Core.Project.MainMenu;
using Data.Configurations.Level;
using Infrastructure.Services.GameSetup;
using Infrastructure.Services.ProjectManagement;
using Infrastructure.Services.Weapon;
using Infrastructure.Services.Window;
using UI.HandMenu;

#endregion

namespace Core.Project.Gameplay
{
    public class GameplayState : IState, IEnterableWithArg<LevelData>
    {
        private readonly IProjectManagementService _projectManagementService;
        private readonly IGameplaySetupService _gameplaySetupService;
        private readonly IWindowService _windowService;
        private readonly IWeaponService _weaponService;
        private IGameplayLevel _gameplayLevel;

        public GameplayState(IProjectManagementService projectManagementService,
            IGameplaySetupService gameplaySetupService, IWindowService windowService, IWeaponService weaponService)
        {
            _projectManagementService = projectManagementService;
            _gameplaySetupService = gameplaySetupService;
            _windowService = windowService;
            _weaponService = weaponService;
        }

        public async void OnEnter(LevelData levelData)
        {
            await _gameplaySetupService.SetupGameplayAsync(levelData);
            SetupEventHandlers();
        }

        private void SetupEventHandlers()
        {
            _windowService.Get<HandMenuUI>(WindowID.HandMenu).OnExitButtonClicked += ExitInMainMenu;
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

        private async void ExitInMainMenu()
        {
            _gameplayLevel?.StopGame();

            await _gameplaySetupService.CleanupGameplayAsync();

            _projectManagementService.ChangeState<MainMenuState>();
        }
    }
}