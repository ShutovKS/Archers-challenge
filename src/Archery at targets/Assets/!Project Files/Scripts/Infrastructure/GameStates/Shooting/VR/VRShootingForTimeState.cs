using Fitches.ShootingGallery;
using Infrastructure.Services.ProjectStateMachine;
using UI;
using Zenject;

namespace Infrastructure.GameStates.Shooting.VR
{
    public class VRShootingForTimeState : VRShootingBaseState
    {
        private int _timerSeconds = 60;

        [Inject]
        public VRShootingForTimeState(IProjectStateMachineService projectStateMachineService, TargetSpawner targetSpawner,
            InformationDeskUI informationDeskUI) : base(projectStateMachineService, targetSpawner, informationDeskUI)
        {
        }

        protected override void OnSceneInitialized()
        {
            _timerSeconds = 60;

            UpdateTime();
        }

        protected override void UpdateTime()
        {
            _timerSeconds--;
            InformationDeskUI.SetInformationText("Time", $"Время: {_timerSeconds} секунд");

            if (_timerSeconds <= 0)
            {
                ProjectStateMachine.SwitchState<GameMainMenuState>();
            }
        }
    }
}