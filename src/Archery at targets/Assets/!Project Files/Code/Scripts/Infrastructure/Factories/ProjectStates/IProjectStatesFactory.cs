#region

using Infrastructure.Services.ProjectManagement;

#endregion

namespace Infrastructure.Factories.ProjectStates
{
    public interface IProjectStatesFactory
    {
        IState CreateProjectState<TState>() where TState : IState;
    }
}