using Infrastructure.Services.ProjectStateMachine;
using JetBrains.Annotations;

namespace Infrastructure.ProjectStates
{
    [UsedImplicitly]
    public class InitializeState : IState, IEnterable
    {
        private readonly IProjectStateMachineService _projectStateMachineService;

        public InitializeState(IProjectStateMachineService projectStateMachineService)
        {
            _projectStateMachineService = projectStateMachineService;
        }

        public void OnEnter()
        {
            MoveToMainMenu();
        }
        
        private void MoveToMainMenu()
        {
            _projectStateMachineService.SwitchState<MainMenuLoadingState>();
        }
    }
}