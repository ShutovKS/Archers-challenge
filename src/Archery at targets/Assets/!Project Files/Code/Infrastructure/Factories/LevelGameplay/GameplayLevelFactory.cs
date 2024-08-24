using Infrastructure.GameplayLevels;
using JetBrains.Annotations;
using Zenject;

namespace Infrastructure.Factories.LevelGameplay
{
    [UsedImplicitly]
    public class GameplayLevelFactory : IGameplayLevelFactory
    {
        private readonly DiContainer _container;

        public GameplayLevelFactory(DiContainer container)
        {
            _container = container;
        }

        public void Create<T>() where T : IGameplayLevel
        {
            if (_container.HasBinding<IGameplayLevel>())
            {
                _container.Unbind<IGameplayLevel>();
            }

            _container.Bind<IGameplayLevel>().To<T>().AsTransient();
        }
    }
}