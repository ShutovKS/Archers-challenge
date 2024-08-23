using System;
using System.Threading.Tasks;
using Data.SceneContext;
using Extension;
using Features.PositionsContainer;
using Features.Weapon;
using Infrastructure.Factories.GameObjects;
using Infrastructure.Factories.Player;
using Infrastructure.Factories.Target;
using Infrastructure.Services.AssetsAddressables;
using Infrastructure.Services.InteractorSetup;
using Infrastructure.Services.SceneContainer;
using Infrastructure.Services.Stopwatch;
using Infrastructure.Services.Window;
using JetBrains.Annotations;
using UI;
using Zenject;
using Object = UnityEngine.Object;

namespace Infrastructure.GameplayLevels
{
    [UsedImplicitly, Serializable]
    public class InfiniteModeVRGameplayLevel : IGameplayLevel
    {
        private IInteractorSetupService _interactorSetupService;
        private IStopwatchService _stopwatchService;
        private IWindowService _windowService;
        private IGameObjectFactory _gameObjectFactory;
        private IPlayerFactory _playerFactory;
        private ITargetFactory _targetFactory;
        private ISceneContextProvider _sceneContextProvider;

        private InfiniteVRSceneContextData _sceneContextData;

        private InformationDeskUI _infoScreen;
        private PositionsContainer _positionsContainer;
        private Bow _bow;

        private int _targetCount;

        [Inject]
        public void Construct(
            IInteractorSetupService interactorSetupService,
            IStopwatchService stopwatchService,
            IWindowService windowService,
            IGameObjectFactory gameObjectFactory,
            IPlayerFactory playerFactory,
            ITargetFactory targetFactory,
            ISceneContextProvider sceneContextProvider
        )
        {
            _interactorSetupService = interactorSetupService;
            _stopwatchService = stopwatchService;
            _windowService = windowService;
            _gameObjectFactory = gameObjectFactory;
            _playerFactory = playerFactory;
            _targetFactory = targetFactory;
            _sceneContextProvider = sceneContextProvider;
        }

        public event Action<GameResult> OnGameFinished;

        public async Task StartGame()
        {
            GetSceneContextData();
            ConfigurePlayer();
            await InstantiateBow();
            await InstantiateInfoScreen();
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
            _interactorSetupService.SetInteractor(InteractorType.NearFar);

            _playerFactory.Player.SetPositionAndRotation(_sceneContextData.PlayerSpawnPoint);
        }

        private async Task InstantiateBow()
        {
            _bow = await _gameObjectFactory.InstantiateAndGetComponent<Bow>(AssetsAddressableConstants.BOW_PREFAB);
            _bow.gameObject.SetPositionAndRotation(_sceneContextData.BowSpawnPoint);
        }

        private async Task InstantiateInfoScreen()
        {
            _infoScreen = await _windowService.OpenAndGet<InformationDeskUI>(WindowID.InformationDesk);
            _infoScreen.gameObject.SetPositionAndRotation(_sceneContextData.InfoScreenSpawnPoint);
            _infoScreen.SetInformationText("Time", "Time: 0.00");
            _infoScreen.SetInformationText("Score", $"Score count: {_targetCount}");
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
            _bow.OnSelected += StartStopwatch;

            return;

            void StartStopwatch(bool isSelect)
            {
                if (!isSelect) return;

                _stopwatchService.Start();
                _bow.OnSelected -= StartStopwatch;
            }
        }

        private void UpdateInfoScreen(float time)
        {
            _infoScreen.SetInformationText("Time", $"Time: {time:0.00}");
        }

        private async void OnTargetHit(string targetId)
        {
            _targetFactory.TargetHit -= OnTargetHit;
            _targetFactory.Destroy(targetId);

            _targetCount++;
            _infoScreen.SetInformationText("Score", $"Score count: {_targetCount}");

            await InstantiateTarget();
        }

        private void ExitInMainMenu()
        {
            StopStopwatch();
            CloseScreen();
            DestroyObjects();

            OnGameFinished?.Invoke(GameResult.Exit);
        }

        private void StopStopwatch()
        {
            _stopwatchService.OnTick -= UpdateInfoScreen;
            _stopwatchService.Stop();
        }

        private void CloseScreen()
        {
            _windowService.Close(WindowID.InformationDesk);
        }

        private void DestroyObjects()
        {
            _targetFactory.DestroyAll();
            Object.Destroy(_bow.gameObject);
        }
    }
}