#region

using Core.Gameplay;
using Data.Gameplay;

#endregion

namespace Infrastructure.Factories.GameplayLevels
{
    public interface IGameplayLevelsFactory
    {
        IGameplayLevel Create(GameplayMode gameplayMode);
        IGameplayLevel Create<T>() where T : IGameplayLevel;
    }
}