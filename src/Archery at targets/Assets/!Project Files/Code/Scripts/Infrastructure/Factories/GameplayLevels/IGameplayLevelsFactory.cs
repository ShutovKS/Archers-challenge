#region

using Core.Gameplay;
using Data.Configurations.GameplayMode;

#endregion

namespace Infrastructure.Factories.GameplayLevels
{
    public interface IGameplayLevelsFactory
    {
        IGameplayLevel Create(GameplayModeType gameplayModeType);
        IGameplayLevel Create<T>() where T : IGameplayLevel;
    }
}