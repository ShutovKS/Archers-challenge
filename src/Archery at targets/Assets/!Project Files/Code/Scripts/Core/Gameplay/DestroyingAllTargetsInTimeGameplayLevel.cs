using System;
using System.Threading.Tasks;
using Data.Gameplay;
using Data.SceneContext;
using Features.PositionsContainer;
using Features.Weapon;
using Infrastructure.Factories.Player;
using Infrastructure.Factories.Target;
using Infrastructure.Factories.Weapon;
using Infrastructure.Services.Player;
using Infrastructure.Services.SceneContainer;
using Infrastructure.Services.Timer;
using Infrastructure.Services.Window;
using JetBrains.Annotations;
using UI.HandMenu;
using UI.InformationDesk;
using UnityEngine;
using Zenject;

namespace Logics.GameplayLevels
{
    [UsedImplicitly]
    public class DestroyingAllTargetsInTimeGameplayLevel : IGameplayLevel
    {
        public event Action<GameResult> OnGameFinished;

        private ITimerService _timerService;
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
        private float _timeLimit;

        [Inject]
        public void Construct(
            ITimerService timerService,
            IWindowService windowService,
            IPlayerService playerService,
            ITargetFactory targetFactory,
            IWeaponFactory weaponFactory,
            ISceneContextProvider sceneContextProvider
        )
        {
            _timerService = timerService;
            _windowService = windowService;
            _playerService = playerService;
            _targetFactory = targetFactory;
            _weaponFactory = weaponFactory;
            _sceneContextProvider = sceneContextProvider;
        }

        public async Task StartGame<TGameplayModeData>(TGameplayModeData gameplayModeData)
            where TGameplayModeData : GameplayModeData
        {
            if (gameplayModeData is not DestroyingAllTargetsInTimeGameplay modeData)
            {
                Debug.LogError("Invalid gameplay mode data type");
                throw new ArgumentException("Invalid gameplay mode data type");
            }

            _targetCount = modeData.TargetCount;
            _timeLimit = modeData.TimeLimit;

            GetSceneContextData();
            ConfigurePlayer();
            await InstantiateWeapon();
            await InstantiateInfoScreen();
            await InstantiateHandMenuScreen();
            await InstantiateTargets(modeData.TargetCount);
            StartTimerOnSelectBow();
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
                _sceneContextData.PlayerSpawnPoint.rotation);
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
            _infoScreen.SetTimeText(_timeLimit.ToString("0.00"));
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

        private void StartTimerOnSelectBow()
        {
            _timerService.OnTick += UpdateInfoScreen;
            _weapon.OnSelected += StartTimer;
        }

        private void StartTimer(bool isSelect)
        {
            if (!isSelect) return;

            _timerService.Start(_timeLimit);
            _weapon.OnSelected -= StartTimer;
            _timerService.OnFinished += OnTimeIsUp;
        }

        private void UpdateInfoScreen(float time)
        {
            _infoScreen.SetTimeText(time.ToString("0.00"));
        }

        private void OnTimeIsUp()
        {
            StopGame();

            OnGameFinished?.Invoke(GameResult.Lose);
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
            StopTimer();
            CloseUpdateInfoScreen();
            CloseHandMenuScreen();
            DestroyTargets();
            DestroyBow();
        }

        private void StopTimer()
        {
            _timerService.OnTick -= UpdateInfoScreen;
            _timerService.Stop();
        }

        private void CloseUpdateInfoScreen()
        {
            _timerService.OnTick -= UpdateInfoScreen;
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
            _weapon.OnSelected -= StartTimer;
            _weaponFactory.Destroy(_weapon);
        }
    }
}