#region

using System;
using Core.Gameplay;
using Data.Configurations.Level;
using Zenject;
using static Data.Configurations.Level.GameplayModeType;

#endregion

namespace Infrastructure.Factories.GameplayLevels
{
    public class GameplayLevelsFactory : IGameplayLevelsFactory
    {
        private readonly DiContainer _container;

        public GameplayLevelsFactory(DiContainer container)
        {
            _container = container;
        }

        public IGameplayLevel Create(GameplayModeType gameplayModeType) => gameplayModeType switch
        {
            InfiniteMR => Create<InfiniteModeMRGameplayLevel>(),
            InfiniteVR => Create<InfiniteModeVRGameplayLevel>(),

            DestroyingAllTargets => Create<DestroyingNTargetsGameplayLevel>(),

            None => throw new NotImplementedException($"Gameplay mode {gameplayModeType} is not implemented."),
            _ => throw new ArgumentOutOfRangeException(nameof(gameplayModeType), gameplayModeType, null)
        };

        private IGameplayLevel Create<T>() where T : IGameplayLevel
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