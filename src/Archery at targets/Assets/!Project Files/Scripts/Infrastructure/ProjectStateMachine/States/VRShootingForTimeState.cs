namespace Infrastructure.ProjectStateMachine.States
{
    public class VRShootingForTimeState : VRShootingBaseState
    {
        private int _timerSeconds = 60;

        public VRShootingForTimeState(GameBootstrap initializer) : base(initializer)
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
                Initializer.StateMachine.SwitchState<GameMainMenuState>();
            }
        }
    }
}