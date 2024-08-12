using System;
using System.Threading;
using System.Threading.Tasks;
using Fitches.ShootingGallery;
using Infrastructure.Services.ProjectStateMachine;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using AsyncOperation = UnityEngine.AsyncOperation;

namespace Infrastructure.GameStates.Shooting.VR
{
    public abstract class VRShootingBaseState : IState, IEnterable, IExitable
    {
        protected readonly IProjectStateMachineService ProjectStateMachine;
        protected readonly TargetSpawner TargetSpawner;
        protected readonly InformationDeskUI InformationDeskUI;

        protected int TargetHitCount;
        protected int TimeSeconds;
        protected CancellationTokenSource CancellationTokenSource;

        protected VRShootingBaseState(IProjectStateMachineService projectStateMachine, TargetSpawner targetSpawner,
            InformationDeskUI informationDeskUI)
        {
            ProjectStateMachine = projectStateMachine;
            TargetSpawner = targetSpawner;
            InformationDeskUI = informationDeskUI;
        }

        public void OnEnter()
        {
            CancellationTokenSource = new CancellationTokenSource();
            var loadSceneAsync = SceneManager.LoadSceneAsync("Gameplay-VR");
            loadSceneAsync!.completed += InitScene;
        }

        private void InitScene(AsyncOperation obj)
        {
            if (InformationDeskUI == null)
            {
                Debug.LogError("InformationDeskUI not found");
                return;
            }

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

            CancellationTokenSource.Cancel();
            CancellationTokenSource.Dispose();
        }
    }
}