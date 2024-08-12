using System;

namespace Infrastructure.Services.ProjectStateMachine
{
    public interface IProjectStateMachineService
    {
        void SwitchState<TState>() where TState : IState;
        void SwitchState<TState, T0>(T0 arg) where TState : IState;
        IState GetCurrentState();
        Type GetCurrentStateType();
    }
}