using Infrastructure.GameplayLevels;

namespace Infrastructure.Factories.LevelGameplay
{
    public interface IGameplayLevelFactory
    {
        void Create<T>() where T : IGameplayLevel;
    }
}