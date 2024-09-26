#region

#endregion

namespace Infrastructure.Services.ProjectManagement
{
    public interface IProjectManagementService
    {
        void ChangeState<TState>() where TState : IState;
        void ChangeState<TState, T0>(T0 arg) where TState : IState;
    }
}