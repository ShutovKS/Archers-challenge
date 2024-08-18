using System;

namespace Infrastructure.ProjectStateMachine
{
    public interface IProjectStateMachine
    {
        void SwitchState<TState>() where TState : IState;
        void SwitchState<TState, T0>(T0 arg) where TState : IState;
        IState GetCurrentState();
        Type GetCurrentStateType();
    }
}