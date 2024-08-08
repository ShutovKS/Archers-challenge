using System;
using System.Threading;
using System.Threading.Tasks;
using ProjectStateMachine.Base;
using ShootingGallery;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace ProjectStateMachine.States
{
    public class VRShootingForTimeState : IState<GameBootstrap>, IEnterable, IExitable
    {
        public VRShootingForTimeState(GameBootstrap initializer)
        {
            Initializer = initializer;
        }

        public GameBootstrap Initializer { get; }

        private TargetSpawner _targetSpawner;
        private InformationDeskUI _informationDeskUI;

        private int _timerSeconds = 60;
        private int _targetHitCount;

        private CancellationTokenSource _cancellationTokenSource;

        public void OnEnter()
        {
            _cancellationTokenSource = new CancellationTokenSource();

            var loadSceneAsync = SceneManager.LoadSceneAsync("Gameplay-VR");
            loadSceneAsync!.completed += InitScene;
        }

        private void InitScene(AsyncOperation obj)
        {
            _informationDeskUI = Object.FindAnyObjectByType<InformationDeskUI>();
            if (_informationDeskUI == null)
            {
                Debug.LogError("InformationDeskUI not found");
                return;
            }

            UpdateTime();
            StartTimerUpdate();

            _targetSpawner = Object.FindAnyObjectByType<TargetSpawner>();
            if (_targetSpawner == null)
            {
                Debug.LogError("TargetSpawner not found");
                return;
            }

            _targetSpawner.TargetHit += IncreaseScore;
            _targetSpawner.TargetHit += UpdateScore;
            _targetSpawner.TargetHit += _targetSpawner.SpawnTarget;
            _targetSpawner.SpawnTarget();
            UpdateScore();
        }

        private void UpdateScore()
        {
            _informationDeskUI?.SetInformationText("Score", $"{_targetHitCount} Очков");
        }

        private void IncreaseScore()
        {
            _targetHitCount++;
        }

        private async void StartTimerUpdate()
        {
            var token = _cancellationTokenSource.Token;
            try
            {
                while (!token.IsCancellationRequested && _timerSeconds > 0)
                {
                    await Task.Delay(1000, token);
                    _timerSeconds--;
                    UpdateTime();
                }

                Initializer.StateMachine.SwitchState<GameMainMenuState>();
            }
            catch (Exception)
            {
                // ignored
            }
        }


        private void UpdateTime()
        {
            _informationDeskUI.SetInformationText("Time", $"Время: {_timerSeconds} секунд");
        }

        public void OnExit()
        {
            if (_targetSpawner != null)
            {
                _targetSpawner.TargetHit -= IncreaseScore;
                _targetSpawner.TargetHit -= UpdateScore;
                _targetSpawner.TargetHit -= _targetSpawner.SpawnTarget;
            }

            _targetHitCount = 0;
            _timerSeconds = 60;
            _targetSpawner = null;
            _informationDeskUI = null;

            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }
    }
}