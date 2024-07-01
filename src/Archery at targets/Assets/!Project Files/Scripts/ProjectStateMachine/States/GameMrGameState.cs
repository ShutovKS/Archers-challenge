using ProjectStateMachine.Base;
using ShootingGallery;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;

namespace ProjectStateMachine.States
{
    public class GameMrGameState : IState<GameBootstrap>, IEnterable, IExitable
    {
        public GameMrGameState(GameBootstrap initializer)
        {
            Initializer = initializer;
        }

        public GameBootstrap Initializer { get; }
        private TargetSpawner _targetSpawner;
        private PointCounterUI _pointCounterUI;

        public void OnEnter()
        {
            var loadSceneAsync = SceneManager.LoadSceneAsync("Gameplay-MR");
            loadSceneAsync!.completed += OnSceneLoaded;
        }

        private void OnSceneLoaded(AsyncOperation obj)
        {
            _targetSpawner = Object.FindAnyObjectByType<TargetSpawner>();
            _targetSpawner.OnTargetHitCountChanged += OnTargetHitCountChanged;

            _pointCounterUI = Object.FindAnyObjectByType<PointCounterUI>();

            OnTargetHitCountChanged(0);
        }

        private void OnTargetHitCountChanged(int value)
        {
            _pointCounterUI.SetPointCounter($"{value}/5 Очков");

            if (value >= 5)
            {
                Initializer.StateMachine.SwitchState<GameMainMenuState>();
            }
        }

        public void OnExit()
        {
            Object.FindAnyObjectByType<ARSession>().enabled = false;
        }
    }
}