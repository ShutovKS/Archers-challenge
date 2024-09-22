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
    public class DestroyingNTargetsGameplayLevel : IGameplayLevel
    {
        public event Action<GameResult> OnGameFinished;
        public event Action<GameState> OnGameStateChanged;

        private ISceneContextProvider _sceneContextProvider;
        private IWindowService _windowService;
        private IStopwatchService _stopwatchService;
        private ITargetFactory _targetFactory;

        private PositionsContainer _positionsContainer;
        private InformationDeskUI _infoScreen;
        private int _targetCount;
        private bool _isPaused;

        [Inject]
        public void Construct(
            IStopwatchService stopwatchService,
            ITargetFactory targetFactory,
            ISceneContextProvider sceneContextProvider,
            IWindowService windowService)
        {
            _stopwatchService = stopwatchService;
            _targetFactory = targetFactory;
            _sceneContextProvider = sceneContextProvider;
            _windowService = windowService;
        }

        public Task PrepareGame<TGameplayModeData>(TGameplayModeData gameplayModeData)
            where TGameplayModeData : GameplayModeData
        {
            _infoScreen = _windowService.Get<InformationDeskUI>(WindowID.InformationDesk);
            var sceneContextData = _sceneContextProvider.Get<GameplaySceneContextData>();
            _positionsContainer = sceneContextData.PositionsContainer;

            if (gameplayModeData is not DestroyingNTargetsGameplay destroyingNTargetsGameplay)
            {
                throw new ArgumentException("Invalid gameplay mode data type");
            }

            _targetCount = destroyingNTargetsGameplay.TargetCount;

            return Task.CompletedTask;
        }

        public async Task StartGame()
        {
            await InstantiateTarget();

            StartStopwatch();

            OnGameStateChanged?.Invoke(GameState.Running);
        }


        #region StartGame

        private async Task InstantiateTarget()
        {
            var (position, rotation) = _positionsContainer.GetTargetPosition();
            await _targetFactory.Instantiate(position, rotation);
            _targetFactory.TargetHit += OnTargetHit;
        }

        private async void OnTargetHit(GameObject gameObject)
        {
            _targetFactory.TargetHit -= OnTargetHit;
            _targetFactory.Destroy(gameObject);

            _targetCount--;
            _infoScreen.SetScoreText(_targetCount.ToString());

            if (_targetCount == 0)
            {
                OnGameStateChanged?.Invoke(GameState.Finished);
                OnGameFinished?.Invoke(GameResult.Win);

                return;
            }

            await InstantiateTarget();
        }

        private void StartStopwatch()
        {
            _stopwatchService.Start();
            _stopwatchService.OnTick += UpdateInfoScreen;
        }

        private void UpdateInfoScreen(float time)
        {
            _infoScreen.SetTimeText(time.ToString("0.00"));
        }

        #endregion

        public void PauseGame()
        {
            if (!_isPaused)
            {
                _stopwatchService.Pause();
                _isPaused = true;
                OnGameStateChanged?.Invoke(GameState.Paused);
            }
        }

        public void ResumeGame()
        {
            if (_isPaused)
            {
                _stopwatchService.Resume();
                _isPaused = false;
                OnGameStateChanged?.Invoke(GameState.Running);
            }
        }

        public void StopGame()
        {
            OnGameStateChanged?.Invoke(GameState.Finished);
        }

        public void CleanUp()
        {
            StopStopwatch();
            DestroyTargets();
        }

        #region CleanUp

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

        #endregion
    }
}