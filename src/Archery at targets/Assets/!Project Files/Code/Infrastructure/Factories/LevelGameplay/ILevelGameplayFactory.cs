using Infrastructure.GameplayLevels;
using JetBrains.Annotations;
using Zenject;

namespace Infrastructure.Factories.LevelGameplay
{
    public interface IGameplayLevelFactory
    {
        void Create<T>() where T : IGameplayLevel;
    }

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
            _container.Bind<IGameplayLevel>().To<T>().AsTransient();
        }
    }
}