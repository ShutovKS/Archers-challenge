using ProjectStateMachine.Base;
using ProjectStateMachine.States;
using UnityEngine;

namespace ProjectStateMachine
{
    public class GameBootstrap : MonoBehaviour
    {
        private void Start()
        {
            StateMachine = new StateMachine<GameBootstrap>(
                new BootstrapState(this),
                new GameMainMenuState(this),
                new VRShootingPerNumberHitsState(this),
                // new VRShootingForTimeState(this),
                // new VRShootingInfiniteState(this),
                new MRShootingPerNumberHitsState(this)
                // new MRShootingForTimeState(this),
                // new MRShootingInfiniteState(this)
            );

            StateMachine.SwitchState<BootstrapState>();
        }

        public StateMachine<GameBootstrap> StateMachine;
    }
}