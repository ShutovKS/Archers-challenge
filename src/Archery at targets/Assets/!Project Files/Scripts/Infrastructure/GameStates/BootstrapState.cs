using Infrastructure.Services.ProjectStateMachine;

namespace Infrastructure.GameStates
{
    public class BootstrapState : IState, IEnterable
    {
        private readonly IProjectStateMachineService _projectStateMachine;

        public BootstrapState(IProjectStateMachineService projectStateMachine)
        {
            _projectStateMachine = projectStateMachine;
        }

        public void OnEnter()
        {
            _projectStateMachine.SwitchState<GameMainMenuState>();
        }
    }
}