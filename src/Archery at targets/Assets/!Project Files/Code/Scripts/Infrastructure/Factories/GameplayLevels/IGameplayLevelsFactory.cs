#region

using Data.Gameplay;
using Logics.GameplayLevels;

#endregion

namespace Infrastructure.Factories.GameplayLevels
{
    public interface IGameplayLevelsFactory
    {
        IGameplayLevel Create(GameplayMode gameplayMode);
        IGameplayLevel Create<T>() where T : IGameplayLevel;
    }
}