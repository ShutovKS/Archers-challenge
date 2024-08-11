namespace ProjectStateMachine.States
{
    public class VRShootingPerNumberHitsState : VRShootingBaseState
    {
        private int _targetHitCountMax = 5;

        public VRShootingPerNumberHitsState(GameBootstrap initializer) : base(initializer)
        {
        }

        protected override void OnSceneInitialized()
        {
            // Логика для проверки количества попаданий
        }

        protected override void UpdateScore()
        {
            InformationDeskUI?.SetInformationText("Score", $"{TargetHitCount}/{_targetHitCountMax} Очков");

            if (TargetHitCount >= _targetHitCountMax)
            {
                Initializer.StateMachine.SwitchState<GameMainMenuState>();
            }
        }
    }
}