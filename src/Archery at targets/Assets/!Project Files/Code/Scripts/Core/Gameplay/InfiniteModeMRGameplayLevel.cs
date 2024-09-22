using System;
using System.Threading.Tasks;
using Data.Configurations.Level;
using Data.Contexts.Scene;
using Features.PositionsContainer;
using Features.Weapon;
using Infrastructure.Factories.ARComponents;
using Infrastructure.Factories.Target;
using Infrastructure.Providers.SceneContainer;
using Infrastructure.Services.Stopwatch;
using Infrastructure.Services.Window;
using UI.HandMenu;
using UI.InformationDesk;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.OpenXR.Features.Meta;
using Zenject;

namespace Core.Gameplay
{
    public class InfiniteModeMRGameplayLevel : IGameplayLevel
    {
        public event Action<GameResult> OnGameFinished;
        public event Action<GameState> OnGameStateChanged;

        private IStopwatchService _stopwatchService;
        private IWindowService _windowService;
        private ITargetFactory _targetFactory;
        private ISceneContextProvider _sceneContextProvider;
        private IARComponentsFactory _arComponentsFactory;

        private GameplaySceneContextData _sceneContextData;

        private HandMenuUI _handMenuScreen;
        private InformationDeskUI _infoScreen;
        private PositionsContainer _positionsContainer;
        private IWeapon _weapon;

        private int _targetCount;
        private bool _isPaused;

        [Inject]
        public void Construct(
            IStopwatchService stopwatchService,
            IWindowService windowService,
            ITargetFactory targetFactory,
            ISceneContextProvider sceneContextProvider,
            IARComponentsFactory arComponentsFactory
        )
        {
            _stopwatchService = stopwatchService;
            _windowService = windowService;
            _targetFactory = targetFactory;
            _sceneContextProvider = sceneContextProvider;
            _arComponentsFactory = arComponentsFactory;
        }


        public Task PrepareGame<TGameplayModeData>(TGameplayModeData gameplayModeData)
            where TGameplayModeData : GameplayModeData
        {
            if (!TryRequestSceneCapture())
            {
                OnGameFinished?.Invoke(GameResult.Error);

                return Task.CompletedTask;
            }

            _infoScreen = _windowService.Get<InformationDeskUI>(WindowID.InformationDesk);
            var sceneContextData = _sceneContextProvider.Get<GameplaySceneContextData>();
            _positionsContainer = sceneContextData.PositionsContainer;

            return Task.CompletedTask;
        }

        public async Task StartGame()
        {
            OnGameStateChanged?.Invoke(GameState.Running);

            await InstantiateTarget();

            StartStopwatch();
        }

        #region StartGame

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

            Debug.LogError("ARSession subsystem not MetaOpenXRSessionSubsystem");

            return false;
        }

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

            _targetCount++;
            _infoScreen.SetScoreText(_targetCount.ToString());

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

        public async Task StopGame()
        {
            OnGameStateChanged?.Invoke(GameState.Finished);

            StopStopwatch();
            DestroyTargets();

            OnGameFinished?.Invoke(GameResult.Win);

            await Task.CompletedTask;
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