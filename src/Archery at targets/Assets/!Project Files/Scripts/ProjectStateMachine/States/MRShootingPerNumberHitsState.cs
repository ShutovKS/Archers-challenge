using ProjectStateMachine.Base;
using ShootingGallery;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;

namespace ProjectStateMachine.States
{
    public class MRShootingPerNumberHitsState : IState<GameBootstrap>, IEnterable, IExitable
    {
        public MRShootingPerNumberHitsState(GameBootstrap initializer)
        {
            Initializer = initializer;
        }

        public GameBootstrap Initializer { get; }
        private TargetSpawner _targetSpawner;
        private InformationDeskUI _informationDeskUI;

        public void OnEnter()
        {
            var loadSceneAsync = SceneManager.LoadSceneAsync("Gameplay-MR");
            loadSceneAsync!.completed += OnSceneLoaded;
        }

        private void OnSceneLoaded(AsyncOperation obj)
        {
            _targetSpawner = Object.FindAnyObjectByType<TargetSpawner>();
            _targetSpawner.TargetHit += OnTargetHit;

            _informationDeskUI = Object.FindAnyObjectByType<InformationDeskUI>();

            OnTargetHit();
        }

        private void OnTargetHit()
        {
            var value = 0;
            _informationDeskUI.SetInformationText("Score", $"{value}/5 Очков");

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