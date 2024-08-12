using Fitches.ShootingGallery;
using Infrastructure.Services.ProjectStateMachine;
using UI;
using Zenject;

namespace Infrastructure.GameStates.Shooting.VR
{
    public class VRShootingInfiniteState : VRShootingBaseState
    {
        [Inject]
        public VRShootingInfiniteState(IProjectStateMachineService projectStateMachineService, TargetSpawner targetSpawner,
            InformationDeskUI informationDeskUI) : base(projectStateMachineService, targetSpawner, informationDeskUI)
        {
        }

        protected override void OnSceneInitialized()
        {
        }
    }
}