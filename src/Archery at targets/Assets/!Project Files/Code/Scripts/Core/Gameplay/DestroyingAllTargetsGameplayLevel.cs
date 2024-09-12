using System;
using System.Threading.Tasks;
using Data.Configurations.GameplayMode;
using Data.Contexts.Scene;
using Features.PositionsContainer;
using Features.Weapon;
using Infrastructure.Factories.Target;
using Infrastructure.Factories.Weapon;
using Infrastructure.Providers.SceneContainer;
using Infrastructure.Services.Player;
using Infrastructure.Services.Stopwatch;
using Infrastructure.Services.Window;
using UI.HandMenu;
using UI.InformationDesk;
using UnityEngine;
using Zenject;

namespace Core.Gameplay
{
    public class DestroyingAllTargetsGameplayLevel : IGameplayLevel
    {
        public event Action<GameResult> OnGameFinished;

        private IStopwatchService _stopwatchService;
        private IWindowService _windowService;
        private IPlayerService _playerService;
        private ITargetFactory _targetFactory;
        private IWeaponFactory _weaponFactory;
        private ISceneContextProvider _sceneContextProvider;

        private GameplaySceneContextData _sceneContextData;
        private PositionsContainer _positionsContainer;

        private IWeapon _weapon;
        private HandMenuUI _handMenuScreen;
        private InformationDeskUI _infoScreen;

        private int _targetCount;

        [Inject]
        public void Construct(
            IStopwatchService stopwatchService,
            IWindowService windowService,
            IPlayerService playerService,
            ITargetFactory targetFactory,
            IWeaponFactory weaponFactory,
            ISceneContextProvider sceneContextProvider
        )
        {
            _stopwatchService = stopwatchService;
            _windowService = windowService;
            _playerService = playerService;
            _targetFactory = targetFactory;
            _weaponFactory = weaponFactory;
            _sceneContextProvider = sceneContextProvider;
        }

        public async Task StartGame<TGameplayModeData>(TGameplayModeData gameplayModeData)
            where TGameplayModeData : GameplayModeData
        {
            if (gameplayModeData is not DestroyingAllTargetsGameplay modeData)
            {
                Debug.LogError("Invalid gameplay mode data type");
                throw new ArgumentException("Invalid gameplay mode data type");
            }

            _targetCount = modeData.TargetCount;

            GetSceneContextData();
            ConfigurePlayer();
            await InstantiateWeapon();
            await InstantiateInfoScreen();
            await InstantiateHandMenuScreen();
            await InstantiateTargets(modeData.TargetCount);
            StartStopwatchOnSelectBow();
        }

        private void GetSceneContextData()
        {
            _sceneContextData = _sceneContextProvider.Get<GameplaySceneContextData>();
            _positionsContainer = _sceneContextData.PositionsContainer;
        }

        private void ConfigurePlayer()
        {
            _playerService.SetPlayerPositionAndRotation(
                _sceneContextData.PlayerSpawnPoint.position,
                _sceneContextData.PlayerSpawnPoint.rotation
            );
        }

        private async Task InstantiateWeapon()
        {
            var spawnPoint = _sceneContextData.BowSpawnPoint;

            _weapon = await _weaponFactory.Instantiate(spawnPoint.position, spawnPoint.rotation);
        }

        private async Task InstantiateInfoScreen()
        {
            var spawnPoint = _sceneContextData.InfoScreenSpawnPoint;
            _infoScreen = await _windowService.OpenInWorldAndGet<InformationDeskUI>(WindowID.InformationDesk,
                spawnPoint.position, spawnPoint.rotation);
            _infoScreen.SetTimeText("0.00");
            _infoScreen.SetScoreText(_targetCount.ToString());
        }

        private async Task InstantiateHandMenuScreen()
        {
            var spawnPoint = _playerService.PlayerContainer.HandMenuSpawnPoint;
            _handMenuScreen = await _windowService.OpenInWorldAndGet<HandMenuUI>(WindowID.HandMenu, spawnPoint.position,
                spawnPoint.rotation, spawnPoint);
            _handMenuScreen.OnExitButtonClicked += ExitInMainMenu;
        }

        private async Task InstantiateTargets(int targetCount = 1)
        {
            for (var i = 0; i < targetCount; i++)
            {
                await InstantiateTarget();
            }
        }

        private async Task InstantiateTarget()
        {
            var (position, rotation) = _positionsContainer.GetTargetPosition();
            await _targetFactory.Instantiate(position, rotation);
            _targetFactory.TargetHit += OnTargetHit;
        }

        private void StartStopwatchOnSelectBow()
        {
            _stopwatchService.OnTick += UpdateInfoScreen;
            _weapon.OnSelected += StartStopwatch;
        }

        private void StartStopwatch(bool isSelect)
        {
            if (!isSelect) return;

            _stopwatchService.Start();
            _weapon.OnSelected -= StartStopwatch;
        }

        private void UpdateInfoScreen(float time)
        {
            _infoScreen.SetTimeText(time.ToString("0.00"));
        }

        private void OnTargetHit(GameObject gameObject)
        {
            _targetFactory.TargetHit -= OnTargetHit;
            _targetFactory.Destroy(gameObject);

            _targetCount--;
            _infoScreen.SetScoreText(_targetCount.ToString());

            if (_targetCount == 0)
            {
                StopGame();

                OnGameFinished?.Invoke(GameResult.Win);
            }
        }

        private void ExitInMainMenu()
        {
            StopGame();

            OnGameFinished?.Invoke(GameResult.Exit);
        }

        private void StopGame()
        {
            StopStopwatch();
            CloseUpdateInfoScreen();
            CloseHandMenuScreen();
            DestroyTargets();
            DestroyBow();
        }

        private void StopStopwatch()
        {
            _stopwatchService.OnTick -= UpdateInfoScreen;
            _stopwatchService.Stop();
        }

        private void CloseUpdateInfoScreen()
        {
            _stopwatchService.OnTick -= UpdateInfoScreen;
            _windowService.Close(WindowID.InformationDesk);
        }

        private void CloseHandMenuScreen()
        {
            _handMenuScreen.OnExitButtonClicked -= ExitInMainMenu;
            _windowService.Close(WindowID.HandMenu);
        }

        private void DestroyTargets()
        {
            _targetFactory.TargetHit -= OnTargetHit;
            _targetFactory.DestroyAll();
        }

        private void DestroyBow()
        {
            _weapon.OnSelected -= StartStopwatch;
            _weaponFactory.Destroy(_weapon);
        }
    }
}