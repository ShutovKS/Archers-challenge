#region

using System;

#endregion

namespace Infrastructure.Services.ProjectManagement
{
    public interface IProjectManagementService
    {
        void SwitchState<TState>() where TState : IState;
        void SwitchState<TState, T0>(T0 arg) where TState : IState;
        IState GetCurrentState();
        Type GetCurrentStateType();
    }
}