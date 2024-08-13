using Zenject;

namespace Infrastructure.Services.SceneDependency
{
    public class SceneDependencyProvider : ISceneDependencyProvider
    {
        private readonly DiContainer _sceneContainer;

        public SceneDependencyProvider(DiContainer sceneContainer)
        {
            _sceneContainer = sceneContainer;
        }

        public T GetDependency<T>()
        {
            return _sceneContainer.Resolve<T>();
        }
    }
}