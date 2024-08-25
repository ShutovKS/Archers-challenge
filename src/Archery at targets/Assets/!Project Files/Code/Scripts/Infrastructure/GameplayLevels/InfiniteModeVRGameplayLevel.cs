using System;
using System.Threading.Tasks;
using Data.SceneContext;
using Extension;
using Features.PositionsContainer;
using Features.Weapon;
using Infrastructure.Factories.GameObjects;
using Infrastructure.Factories.Player;
using Infrastructure.Factories.Target;
using Infrastructure.Factories.Weapon;
using Infrastructure.Services.SceneContainer;
using Infrastructure.Services.Stopwatch;
using Infrastructure.Services.Window;
using JetBrains.Annotations;
using UI;
using UnityEngine;
using Zenject;

namespace Infrastructure.GameplayLevels
{
    [UsedImplicitly, Serializable]
    public class InfiniteModeVRGameplayLevel : IGameplayLevel
    {
        private IStopwatchService _stopwatchService;
        private IWindowService _windowService;
        private IGameObjectFactory _gameObjectFactory;
        private IPlayerFactory _playerFactory;
        private ITargetFactory _targetFactory;
        private IWeaponFactory _weaponFactory;
        private ISceneContextProvider _sceneContextProvider;

        private InfiniteVRSceneContextData _sceneContextData;

        private HandMenuUI _handMenuScreen;
        private InformationDeskUI _infoScreen;
        private PositionsContainer _positionsContainer;
        private IWeapon _weapon;

        private int _targetCount;

        [Inject]
        public void Construct(
            IStopwatchService stopwatchService,
            IWindowService windowService,
            IGameObjectFactory gameObjectFactory,
            IPlayerFactory playerFactory,
            ITargetFactory targetFactory,
            IWeaponFactory weaponFactory,
            ISceneContextProvider sceneContextProvider
        )
        {
            _stopwatchService = stopwatchService;
            _windowService = windowService;
            _gameObjectFactory = gameObjectFactory;
            _playerFactory = playerFactory;
            _targetFactory = targetFactory;
            _weaponFactory = weaponFactory;
            _sceneContextProvider = sceneContextProvider;
        }

        public event Action<GameResult> OnGameFinished;

        public async Task StartGame()
        {
            GetSceneContextData();
            ConfigurePlayer();
            await InstantiateWeapon();
            await InstantiateInfoScreen();
            await InstantiateHandMenuScreen();
            await InstantiateTarget();
            StartStopwatchOnSelectBow();
        }

        private void GetSceneContextData()
        {
            _sceneContextData = _sceneContextProvider.Get<InfiniteVRSceneContextData>();
            _positionsContainer = _sceneContextData.PositionsContainer;
        }

        private void ConfigurePlayer()
        {
            _playerFactory.PlayerContainer.Player.SetPositionAndRotation(_sceneContextData.PlayerSpawnPoint);
        }

        private async Task InstantiateWeapon()
        {
            var spawnPoint = _sceneContextData.BowSpawnPoint;

            _weapon = await _weaponFactory.Instantiate(spawnPoint.position, spawnPoint.rotation);
        }

        private async Task InstantiateInfoScreen()
        {
            var spawnPoint = _sceneContextData.InfoScreenSpawnPoint;
            _infoScreen = await _windowService.OpenAndGet<InformationDeskUI>(WindowID.InformationDesk,
                spawnPoint.position, spawnPoint.rotation);
            _infoScreen.SetInformationText("Time", "Time: 0.00");
            _infoScreen.SetInformationText("Score", $"Score count: {_targetCount}");
        }

        private async Task InstantiateHandMenuScreen()
        {
            var spawnPoint = _playerFactory.PlayerContainer.HandMenuSpawnPoint;
            _handMenuScreen = await _windowService.OpenAndGet<HandMenuUI>(WindowID.HandMenu, spawnPoint.position,
                spawnPoint.rotation, spawnPoint);
            _handMenuScreen.OnExitButtonClicked += ExitInMainMenu;
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
            _infoScreen.SetInformationText("Time", $"Time: {time:0.00}");
        }

        private async void OnTargetHit(GameObject gameObject)
        {
            _targetFactory.TargetHit -= OnTargetHit;
            _targetFactory.Destroy(gameObject);

            _targetCount++;
            _infoScreen.SetInformationText("Score", $"Score count: {_targetCount}");

            await InstantiateTarget();
        }

        private void ExitInMainMenu()
        {
            StopStopwatch();
            CloseUpdateInfoScreen();
            CloseHandMenuScreen();
            DestroyTargets();
            DestroyBow();

            OnGameFinished?.Invoke(GameResult.Exit);
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