using Infrastructure.ProjectStateMachine;
using JetBrains.Annotations;

namespace Infrastructure.ProjectStates
{
    [UsedImplicitly]
    public class BootstrapState : IState, IEnterable
    {
        private readonly IProjectStateMachine _projectStateMachine;

        public BootstrapState(IProjectStateMachine projectStateMachine)
        {
            _projectStateMachine = projectStateMachine;
        }

        public void OnEnter()
        {
            _projectStateMachine.SwitchState<InitializeState>();
        }
    }
}