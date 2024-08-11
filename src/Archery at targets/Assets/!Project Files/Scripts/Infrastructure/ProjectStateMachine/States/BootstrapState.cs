using Infrastructure.ProjectStateMachine.Base;

namespace Infrastructure.ProjectStateMachine.States
{
    public class BootstrapState : IState<GameBootstrap>, IEnterable
    {
        public GameBootstrap Initializer { get; }

        public BootstrapState(GameBootstrap initializer)
        {
            Initializer = initializer;
        }

        public void OnEnter()
        {
            Initializer.StateMachine.SwitchState<GameMainMenuState>();
        }
    }
}