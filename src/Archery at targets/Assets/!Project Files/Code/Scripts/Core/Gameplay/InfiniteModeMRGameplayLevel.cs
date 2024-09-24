using System;
using System.Threading.Tasks;
using Data.Configurations.Level;
using Data.Contexts.Scene;
using Features.TargetsInLevelManager;
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
        private TargetsInLevelManager _targetsInLevelManager;
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
            _targetsInLevelManager = sceneContextData.TargetsInLevelManager;

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

        private void OnTargetHit()
        {
            _targetCount++;
            _infoScreen.SetScoreText(_targetCount.ToString());
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