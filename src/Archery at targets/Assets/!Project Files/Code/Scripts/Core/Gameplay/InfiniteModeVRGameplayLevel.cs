using System;
using System.Threading.Tasks;
using Data.Configurations.Level;
using Data.Contexts.Scene;
using Features.PositionsContainer;
using Infrastructure.Factories.Target;
using Infrastructure.Providers.SceneContainer;
using Infrastructure.Services.Stopwatch;
using Infrastructure.Services.Window;
using UI.InformationDesk;
using UnityEngine;
using Zenject;

namespace Core.Gameplay
{
    public class InfiniteModeVRGameplayLevel : IGameplayLevel
    {
        private ISceneContextProvider _sceneContextProvider;
        private WindowService _windowService;
        private IStopwatchService _stopwatchService;
        private ITargetFactory _targetFactory;

        private PositionsContainer _positionsContainer;
        private InformationDeskUI _infoScreen;
        private int _targetCount;

        [Inject]
        public void Construct(
            IStopwatchService stopwatchService,
            ITargetFactory targetFactory,
            ISceneContextProvider sceneContextProvider,
            WindowService windowService)
        {
            _stopwatchService = stopwatchService;
            _targetFactory = targetFactory;
            _sceneContextProvider = sceneContextProvider;
            _windowService = windowService;
        }

        public event Action<GameResult> OnGameFinished;

        public async Task StartGame<TGameplayModeData>(TGameplayModeData gameplayModeData)
            where TGameplayModeData : GameplayModeData
        {
            _infoScreen = _windowService.Get<InformationDeskUI>(WindowID.InformationDesk);
            var sceneContextData = _sceneContextProvider.Get<InfiniteSceneContextData>();
            _positionsContainer = sceneContextData.PositionsContainer;

            await InstantiateTarget();

            _stopwatchService.Start();
            _stopwatchService.OnTick += UpdateInfoScreen;
        }

        private async Task InstantiateTarget()
        {
            var (position, rotation) = _positionsContainer.GetTargetPosition();

            await _targetFactory.Instantiate(position, rotation);

            _targetFactory.TargetHit += OnTargetHit;
        }

        private void UpdateInfoScreen(float time) => _infoScreen.SetTimeText(time.ToString("0.00"));

        private async void OnTargetHit(GameObject gameObject)
        {
            _targetFactory.TargetHit -= OnTargetHit;
            _targetFactory.Destroy(gameObject);

            _targetCount++;

            _infoScreen.SetScoreText(_targetCount.ToString());

            await InstantiateTarget();
        }

        public void Dispose()
        {
            StopStopwatch();
            DestroyTargets();
        }

        private void StopStopwatch()
        {
            _stopwatchService.OnTick -= UpdateInfoScreen;
            _stopwatchService.Stop();
        }

        private void DestroyTargets()
        {
            _targetFactory.TargetHit -= OnTargetHit;
            _targetFactory.DestroyAll();
        }
    }
}