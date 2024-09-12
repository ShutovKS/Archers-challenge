#region

using Core.Gameplay;
using Data.Configurations.Level;

#endregion

namespace Infrastructure.Factories.GameplayLevels
{
    public interface IGameplayLevelsFactory
    {
        IGameplayLevel Create(GameplayModeType gameplayModeType);
    }
}