using System;
using Infrastructure.GameStates.Shooting.MR;
using Infrastructure.GameStates.Shooting.VR;
using Infrastructure.Services.ProjectStateMachine;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure.GameStates
{
    public class LoadScenesState : IState, IEnterableWithOneArg<string>
    {
        private readonly IProjectStateMachineService _projectStateMachineService;
        private AsyncOperation _asyncOperation;
        private string _stateName;

        public LoadScenesState(IProjectStateMachineService projectStateMachineService)
        {
            _projectStateMachineService = projectStateMachineService;
        }

        public void OnEnter(string stateName)
        {
            _stateName = stateName;

            _asyncOperation = LoadScene();

            _asyncOperation.completed += SwitchState;
        }

        private AsyncOperation LoadScene()
        {
            var sceneName = _stateName switch
            {
                nameof(GameMainMenuState)
                    => "MainMenu",
                nameof(VRShootingForTimeState)
                    or nameof(VRShootingInfiniteState)
                    or nameof(VRShootingPerNumberHitsState)
                    => "Gameplay-VR",
                nameof(MRShootingPerNumberHitsState)
                    => "Gameplay-MR",
                _ => throw new NullReferenceException($"No scene name for state: {_stateName}")
            };

            return SceneManager.LoadSceneAsync(sceneName);
        }

        private void SwitchState(AsyncOperation asyncOperation)
        {
            switch (_stateName)
            {
                case nameof(GameMainMenuState):
                    _projectStateMachineService.SwitchState<GameMainMenuState>();
                    break;
                case nameof(VRShootingForTimeState):
                    _projectStateMachineService.SwitchState<VRShootingForTimeState>();
                    break;
                case nameof(VRShootingInfiniteState):
                    _projectStateMachineService.SwitchState<VRShootingInfiniteState>();
                    break;
                case nameof(VRShootingPerNumberHitsState):
                    _projectStateMachineService.SwitchState<VRShootingPerNumberHitsState>();
                    break;
                case nameof(MRShootingPerNumberHitsState):
                    _projectStateMachineService.SwitchState<MRShootingPerNumberHitsState>();
                    break;
                default:
                    throw new NullReferenceException($"No state for scene: {_stateName}");
            }
        }
    }
}