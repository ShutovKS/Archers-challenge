using System;
using System.Threading.Tasks;
using Data.Configurations.Level;
using Data.Contexts.Scene;
using Features.TargetsInLevelManager;
using Infrastructure.Factories.Target;
using Infrastructure.Providers.SceneContainer;
using Infrastructure.Services.Stopwatch;
using Infrastructure.Services.Window;
using UI.InformationDesk;
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

        private TargetsInLevelManager _targetsInLevelManager;
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
            _sceneContextProvider = sceneContextProvider;
            _windowService = windowService;
        }

        public Task PrepareGame<TGameplayModeData>(TGameplayModeData gameplayModeData)
            where TGameplayModeData : GameplayModeData
        {
            _infoScreen = _windowService.Get<InformationDeskUI>(WindowID.InformationDesk);
            var sceneContextData = _sceneContextProvider.Get<GameplaySceneContextData>();
            _targetsInLevelManager = sceneContextData.TargetsInLevelManager;

            if (gameplayModeData is not DestroyingNTargetsGameplay destroyingNTargetsGameplay)
            {
                throw new ArgumentException("Invalid gameplay mode data type");
            }

            _targetCount = destroyingNTargetsGameplay.TargetCount;

            _targetsInLevelManager.PrepareTargets();

            return Task.CompletedTask;
        }

        public Task StartGame()
        {
            _targetsInLevelManager.OnTargetHit += OnTargetHit;
            _targetsInLevelManager.StartTargets();

            StartStopwatch();

            OnGameStateChanged?.Invoke(GameState.Running);

            return Task.CompletedTask;
        }

        #region StartGame

        private void OnTargetHit()
        {
            _targetCount--;

            _infoScreen.SetScoreText(_targetCount.ToString());

            if (_targetCount == 0)
            {
                OnGameStateChanged?.Invoke(GameState.Finished);
                OnGameFinished?.Invoke(GameResult.Win);
            }
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
            _targetsInLevelManager.StopTargets();

            OnGameStateChanged?.Invoke(GameState.Finished);
        }

        public void CleanUp()
        {
            StopStopwatch();
        }

        #region CleanUp

        private void StopStopwatch()
        {
            _stopwatchService.OnTick -= UpdateInfoScreen;
            _stopwatchService.Stop();
        }

        #endregion
    }
}