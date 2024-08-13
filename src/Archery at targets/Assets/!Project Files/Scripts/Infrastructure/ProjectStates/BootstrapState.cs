using Infrastructure.Services.ProjectStateMachine;
using JetBrains.Annotations;

namespace Infrastructure.ProjectStates
{
    [UsedImplicitly]
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