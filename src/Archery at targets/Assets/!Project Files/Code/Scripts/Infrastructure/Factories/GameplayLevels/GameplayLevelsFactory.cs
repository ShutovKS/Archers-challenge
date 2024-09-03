#region

using Data.Gameplay;
using JetBrains.Annotations;
using Logics.GameplayLevels;
using Zenject;
using static Data.Gameplay.GameplayMode;

#endregion

namespace Infrastructure.Factories.GameplayLevels
{
    [UsedImplicitly]
    public class GameplayLevelsFactory : IGameplayLevelsFactory
    {
        private readonly DiContainer _container;

        public GameplayLevelsFactory(DiContainer container)
        {
            _container = container;
        }

        public IGameplayLevel Create(GameplayMode gameplayMode) => gameplayMode switch
        {
            // Mode => CreateGameplayLevel<Mode>(),

            None or _ => throw new System.NotImplementedException($"Gameplay mode {gameplayMode} is not implemented."),
        };

        public IGameplayLevel Create<T>() where T : IGameplayLevel
        {
            var gameplayLevel = _container.Instantiate<T>();

            Bind(gameplayLevel);

            return gameplayLevel;
        }

        private void Bind(IGameplayLevel gameplayLevel)
        {
            if (_container.HasBinding<IGameplayLevel>())
            {
                _container.Unbind<IGameplayLevel>();
            }

            _container.Bind<IGameplayLevel>().FromInstance(gameplayLevel).AsTransient();
        }
    }
}