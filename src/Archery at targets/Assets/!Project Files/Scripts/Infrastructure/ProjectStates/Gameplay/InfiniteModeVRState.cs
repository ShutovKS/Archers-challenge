using System.Threading.Tasks;
using Data.Level;
using Data.SceneContext;
using Extension;
using Features.PositionsContainer;
using Infrastructure.Factories.GameObjects;
using Infrastructure.Factories.Player;
using Infrastructure.Factories.Target;
using Infrastructure.ProjectStateMachine;
using Infrastructure.Services.AssetsAddressables;
using Infrastructure.Services.InteractorSetup;
using Infrastructure.Services.SceneContainer;
using Infrastructure.Services.SceneLoader;
using Infrastructure.Services.StaticData;
using Infrastructure.Services.Stopwatch;
using Infrastructure.Services.Window;
using Infrastructure.Services.XRSetup;
using Infrastructure.Services.XRSetup.TrackingMode;
using JetBrains.Annotations;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure.ProjectStates.Gameplay
{
    [UsedImplicitly]
    public class InfiniteModeVRState : IState, IEnterable, IExitable
    {
        private readonly IXRSetupService _xrSetupService;
        private readonly IInteractorSetupService _interactorSetupService;
        private readonly IGameObjectFactory _gameObjectFactory;
        private readonly IPlayerFactory _playerFactory;
        private readonly IStaticDataService _staticDataService;
        private readonly ISceneContextProvider _sceneContextProvider;
        private readonly IWindowService _windowService;
        private readonly ISceneLoaderService _sceneLoaderService;
        private readonly ITargetFactory _targetFactory;
        private readonly IStopwatchService _stopwatchService;

        private InfiniteVRSceneContextData _sceneContextData;
        private LevelData _levelData;
        private GameObject _locationInstance;
        private InformationDeskUI _infoScreen;
        private PositionsContainer _positionsContainer;
        private int _targetCount;

        public InfiniteModeVRState(IXRSetupService xrSetupService,
            IInteractorSetupService interactorSetupService,
            IGameObjectFactory gameObjectFactory,
            IPlayerFactory playerFactory,
            IStaticDataService staticDataService,
            ISceneContextProvider sceneContextProvider,
            IWindowService windowService,
            ISceneLoaderService sceneLoaderService,
            ITargetFactory targetFactory,
            IStopwatchService stopwatchService)
        {
            _xrSetupService = xrSetupService;
            _interactorSetupService = interactorSetupService;
            _gameObjectFactory = gameObjectFactory;
            _playerFactory = playerFactory;
            _staticDataService = staticDataService;
            _sceneContextProvider = sceneContextProvider;
            _windowService = windowService;
            _sceneLoaderService = sceneLoaderService;
            _targetFactory = targetFactory;
            _stopwatchService = stopwatchService;
        }

        public async void OnEnter()
        {
            GetLevelData();
            await InstantiateLocation();
            GetSceneContextData();
            ConfigurePlayer();
            await InstantiateBow();
            await InstantiateInfoScreen();
            await InstantiateTarget();
            StartStopwatch();
        }

        private void GetLevelData()
        {
            _levelData = _staticDataService.GetLevelData<LevelData>("InfiniteVR");
        }

        private async Task InstantiateLocation()
        {
            await _sceneLoaderService.LoadSceneAsync(_levelData.LocationSceneReference, LoadSceneMode.Additive);
        }

        private void GetSceneContextData()
        {
            _sceneContextData = _sceneContextProvider.Get<InfiniteVRSceneContextData>();
            _positionsContainer = _sceneContextData.PositionsContainer;
        }

        private void ConfigurePlayer()
        {
            _xrSetupService.SetXRMode(XRMode.VR);
            _xrSetupService.SetXRTrackingMode(new NoneTrackingMode());
            _xrSetupService.SetAnchorManagerState(false);

            _interactorSetupService.SetInteractor(InteractorType.NearFar);

            _playerFactory.Player.SetPositionAndRotation(_sceneContextData.PlayerSpawnPoint);
        }

        private async Task InstantiateBow()
        {
            var bow = await _gameObjectFactory.Instance(AssetsAddressableConstants.BOW_PREFAB);
            bow.SetPositionAndRotation(_sceneContextData.BowSpawnPoint);
        }

        private async Task InstantiateInfoScreen()
        {
            _infoScreen = await _windowService.OpenAndGet<InformationDeskUI>(WindowID.InformationDesk);
            _infoScreen.gameObject.SetPositionAndRotation(_sceneContextData.InfoScreenSpawnPoint);
            _infoScreen.SetInformationText("Time", $"Time: 0.00");
            _infoScreen.SetInformationText("Score", $"Score count: {_targetCount}");
        }

        private async Task InstantiateTarget()
        {
            var (position, rotation) = _positionsContainer.GetTargetPosition();
            await _targetFactory.Instance(position, rotation);
            _targetFactory.TargetHit += OnTargetHit;
        }

        private void StartStopwatch()
        {
            _stopwatchService.OnTick += UpdateInfoScreen;
            _stopwatchService.Start();
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

        public void OnExit()
        {
            StopStopwatch();
            CloseScreen();
            DestroyLocation();
            _targetFactory.DestroyAll();
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

        private void DestroyLocation()
        {
            _sceneLoaderService.UnloadSceneAsync(_levelData.LocationSceneReference);
        }
    }
}