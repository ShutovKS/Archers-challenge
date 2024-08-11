using Infrastructure.ProjectStateMachine.Base;
using Infrastructure.ProjectStateMachine.States;
using Zenject;

namespace Infrastructure.ProjectStateMachine
{
    public class GameBootstrap : IInitializable
    {
        public GameBootstrap()
        {
            StateMachine = new StateMachine<GameBootstrap>(
                new BootstrapState(this),
                new GameMainMenuState(this),
                new VRShootingPerNumberHitsState(this),
                new VRShootingForTimeState(this),
                new VRShootingInfiniteState(this),
                new MRShootingPerNumberHitsState(this)
                // new MRShootingForTimeState(this),
                // new MRShootingInfiniteState(this)
            );
        }

        public readonly StateMachine<GameBootstrap> StateMachine;

        public void Initialize()
        {
            StateMachine.SwitchState<BootstrapState>();
        }
    }
}