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
    public abstract class VRShootingBaseState : IState<GameBootstrap>, IEnterable, IExitable
    {
        protected VRShootingBaseState(GameBootstrap initializer)
        {
            Initializer = initializer;
        }

        public GameBootstrap Initializer { get; }

        protected TargetSpawner TargetSpawner;
        protected InformationDeskUI InformationDeskUI;

        protected int TargetHitCount;
        protected int TimeSeconds;
        protected CancellationTokenSource CancellationTokenSource;

        public void OnEnter()
        {
            CancellationTokenSource = new CancellationTokenSource();
            var loadSceneAsync = SceneManager.LoadSceneAsync("Gameplay-VR");
            loadSceneAsync!.completed += InitScene;
        }

        private void InitScene(AsyncOperation obj)
        {
            InformationDeskUI = Object.FindAnyObjectByType<InformationDeskUI>();
            if (InformationDeskUI == null)
            {
                Debug.LogError("InformationDeskUI not found");
                return;
            }

            TargetSpawner = Object.FindAnyObjectByType<TargetSpawner>();
            if (TargetSpawner == null)
            {
                Debug.LogError("TargetSpawner not found");
                return;
            }

            TargetSpawner.TargetHit += IncreaseScore;
            TargetSpawner.TargetHit += UpdateScore;
            TargetSpawner.TargetHit += TargetSpawner.SpawnTarget;
            TargetSpawner.SpawnTarget();
            UpdateScore();

            StartTimeUpdate();
            OnSceneInitialized();
        }

        protected abstract void OnSceneInitialized();

        protected virtual void UpdateScore()
        {
            InformationDeskUI?.SetInformationText("Score", $"{TargetHitCount} Очков");
        }

        protected virtual void IncreaseScore()
        {
            TargetHitCount++;
        }

        private async void StartTimeUpdate()
        {
            var token = CancellationTokenSource.Token;
            try
            {
                while (!token.IsCancellationRequested)
                {
                    await Task.Delay(1000, token);
                    TimeSeconds++;
                    UpdateTime();
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        protected virtual void UpdateTime()
        {
            InformationDeskUI.SetInformationText("Time", $"Время: {TimeSeconds} сек.");
        }

        public virtual void OnExit()
        {
            if (TargetSpawner != null)
            {
                TargetSpawner.TargetHit -= IncreaseScore;
                TargetSpawner.TargetHit -= UpdateScore;
                TargetSpawner.TargetHit -= TargetSpawner.SpawnTarget;
            }

            TargetHitCount = 0;
            TimeSeconds = 0;
            TargetSpawner = null;
            InformationDeskUI = null;

            CancellationTokenSource.Cancel();
            CancellationTokenSource.Dispose();
        }
    }
}