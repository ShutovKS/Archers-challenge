using System;
using System.Threading.Tasks;
using Data.Gameplay;
using Data.SceneContext;
using Extension;
using Features.PositionsContainer;
using Features.Weapon;
using Infrastructure.Factories.ARComponents;
using Infrastructure.Factories.Player;
using Infrastructure.Factories.Target;
using Infrastructure.Factories.Weapon;
using Infrastructure.Services.ARPlanes;
using Infrastructure.Services.SceneContainer;
using Infrastructure.Services.Stopwatch;
using Infrastructure.Services.Window;
using JetBrains.Annotations;
using UI.HandMenu;
using UI.InformationDesk;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.OpenXR.Features.Meta;
using Zenject;

namespace Logics.GameplayLevels
{
    [UsedImplicitly]
    public class InfiniteModeMRGameplayLevel : IGameplayLevel
    {
        private readonly IStopwatchService _stopwatchService;
        private readonly IWindowService _windowService;
        private readonly IPlayerFactory _playerFactory;
        private readonly ITargetFactory _targetFactory;
        private readonly IWeaponFactory _weaponFactory;
        private readonly ISceneContextProvider _sceneContextProvider;
        private readonly IARComponentsFactory _arComponentsFactory;
        private readonly IARPlanesService _arPlanesService;

        private InfiniteSceneContextData _sceneContextData;

        private HandMenuUI _handMenuScreen;
        private InformationDeskUI _infoScreen;
        private PositionsContainer _positionsContainer;
        private IWeapon _weapon;

        private int _targetCount;

        [Inject]
        public InfiniteModeMRGameplayLevel(
            IStopwatchService stopwatchService,
            IWindowService windowService,
            IPlayerFactory playerFactory,
            ITargetFactory targetFactory,
            IWeaponFactory weaponFactory,
            ISceneContextProvider sceneContextProvider,
            IARComponentsFactory arComponentsFactory,
            IARPlanesService arPlanesService
        )
        {
            _stopwatchService = stopwatchService;
            _windowService = windowService;
            _playerFactory = playerFactory;
            _targetFactory = targetFactory;
            _weaponFactory = weaponFactory;
            _sceneContextProvider = sceneContextProvider;
            _arComponentsFactory = arComponentsFactory;
            _arPlanesService = arPlanesService;
        }

        public event Action<GameResult> OnGameFinished;

        public async Task StartGame<TGameplayModeData>(TGameplayModeData gameplayModeData)
            where TGameplayModeData : GameplayModeData
        {
            if (!TryRequestSceneCapture())
            {
                Debug.LogError("Failed to request scene capture");

                OnGameFinished?.Invoke(GameResult.Error);

                return;
            }

            GetSceneContextData();
            ConfigurePlayer();
            await InstantiateWeapon();
            await InstantiateInfoScreen();
            await InstantiateHandMenuScreen();
            await InstantiateTarget();
            StartStopwatchOnSelectBow();
        }

        private bool TryRequestSceneCapture()
        {
            var arSession = _arComponentsFactory.Get<ARSession>();

            if (arSession == null)
            {
                Debug.LogError("ARSession is null");

                return false;
            }

            if (arSession.subsystem is MetaOpenXRSessionSubsystem subsystem)
            {
                return subsystem.TryRequestSceneCapture();
            }

            Debug.LogError("ARSession subsystem is not MetaOpenXRSessionSubsystem");

            return false;
        }

        private void GetSceneContextData()
        {
            _sceneContextData = _sceneContextProvider.Get<InfiniteSceneContextData>();
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
            _infoScreen.SetTimeText("0.00");
            _infoScreen.SetScoreText("0");
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
            if (_arPlanesService.IsPlaneDetected)
            {
                await InstantiateTargetOnPlane();
            }
            else
            {
                _arPlanesService.OnPlaneDetected += OnPlaneDetected;
            }
        }

        private async void OnPlaneDetected()
        {
            if (_arPlanesService.IsPlaneDetected)
            {
                _arPlanesService.OnPlaneDetected -= OnPlaneDetected;
                await InstantiateTargetOnPlane();
            }
        }

        private async Task InstantiateTargetOnPlane()
        {
            var (position, rotation) = _positionsContainer.GetTargetPosition();

            if (position == Vector3.zero)
            {
                _arPlanesService.OnPlaneDetected += OnPlaneDetected;
                return;
            }

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

        private async void OnTargetHit(GameObject gameObject)
        {
            _targetFactory.TargetHit -= OnTargetHit;
            _targetFactory.Destroy(gameObject);

            _targetCount++;
            _infoScreen.SetScoreText(_targetCount.ToString());

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