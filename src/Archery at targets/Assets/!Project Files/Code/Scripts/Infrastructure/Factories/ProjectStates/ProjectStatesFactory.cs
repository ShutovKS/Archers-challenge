#region

using Infrastructure.Services.ProjectManagement;
using JetBrains.Annotations;
using Zenject;

#endregion

namespace Infrastructure.Factories.ProjectStates
{
    [UsedImplicitly]
    public class ProjectStatesFactory : IProjectStatesFactory
    {
        private readonly DiContainer _container;

        public ProjectStatesFactory(DiContainer container)
        {
            _container = container;
        }

        public IState CreateProjectState<TState>() where TState : IState => _container.Instantiate<TState>();
    }
}