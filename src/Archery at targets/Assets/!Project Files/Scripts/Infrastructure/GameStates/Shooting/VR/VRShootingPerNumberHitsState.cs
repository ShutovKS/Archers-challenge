using Fitches.ShootingGallery;
using Infrastructure.Services.ProjectStateMachine;
using UI;

namespace Infrastructure.GameStates.Shooting.VR
{
    public class VRShootingPerNumberHitsState : VRShootingBaseState
    {
        private int _targetHitCountMax = 5;

        public VRShootingPerNumberHitsState(IProjectStateMachineService projectStateMachineService, TargetSpawner targetSpawner,
            InformationDeskUI informationDeskUI) : base(projectStateMachineService, targetSpawner, informationDeskUI)
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
                ProjectStateMachine.SwitchState<GameMainMenuState>();
            }
        }
    }
}