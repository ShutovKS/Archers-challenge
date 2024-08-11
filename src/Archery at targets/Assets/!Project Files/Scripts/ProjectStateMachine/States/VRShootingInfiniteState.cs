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
    public class VRShootingInfiniteState : IState<GameBootstrap>, IEnterable, IExitable
    {
        public VRShootingInfiniteState(GameBootstrap initializer)
        {
            Initializer = initializer;
        }

        public GameBootstrap Initializer { get; }

        private TargetSpawner _targetSpawner;
        private InformationDeskUI _informationDeskUI;

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

            StartTimeUpdate();

            _targetSpawner = Object.FindAnyObjectByType<TargetSpawner>();
            if (_targetSpawner == null)
            {
                Debug.LogError("TargetSpawner not found");
                return;
            }

            _targetSpawner.TargetHit += IncreaseScore;
            
            _targetSpawner.TargetHit += UpdateScore;
            UpdateScore();
            
            _targetSpawner.TargetHit += _targetSpawner.SpawnTarget;
            _targetSpawner.SpawnTarget();
        }

        private void UpdateScore()
        {
            _informationDeskUI?.SetInformationText("Score", $"{_targetHitCount} Очков");
        }

        private void IncreaseScore()
        {
            _targetHitCount++;
        }

        private async void StartTimeUpdate()
        {
            var token = _cancellationTokenSource.Token;
            try
            {
                while (!token.IsCancellationRequested)
                {
                    await Task.Delay(1000, token);
                    UpdateTime();
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void UpdateTime()
        {
            _informationDeskUI.SetInformationText("Time", $"Время: {_targetHitCount} сек.");
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
            _targetSpawner = null;
            _informationDeskUI = null;
        }
    }
}