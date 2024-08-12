using Fitches.ShootingGallery;
using Infrastructure.Services.ProjectStateMachine;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;

namespace Infrastructure.GameStates.Shooting.MR
{
    public class MRShootingPerNumberHitsState : IState, IEnterable, IExitable
    {
        private readonly IProjectStateMachineService _projectStateMachine;
        private readonly TargetSpawner _targetSpawner;
        private readonly InformationDeskUI _informationDeskUI;

        public MRShootingPerNumberHitsState(IProjectStateMachineService projectStateMachine, TargetSpawner targetSpawner,
            InformationDeskUI informationDeskUI)
        {
            _projectStateMachine = projectStateMachine;
            _targetSpawner = targetSpawner;
            _informationDeskUI = informationDeskUI;
        }

        public void OnEnter()
        {
            var loadSceneAsync = SceneManager.LoadSceneAsync("Gameplay-MR");
            loadSceneAsync!.completed += OnSceneLoaded;
        }

        private void OnSceneLoaded(AsyncOperation obj)
        {
            _targetSpawner.TargetHit += OnTargetHit;

            OnTargetHit();
        }

        private void OnTargetHit()
        {
            var value = 0;
            _informationDeskUI.SetInformationText("Score", $"{value}/5 Очков");

            if (value >= 5)
            {
                _projectStateMachine.SwitchState<GameMainMenuState>();
            }
        }

        public void OnExit()
        {
            Object.FindAnyObjectByType<ARSession>().enabled = false;
        }
    }
}