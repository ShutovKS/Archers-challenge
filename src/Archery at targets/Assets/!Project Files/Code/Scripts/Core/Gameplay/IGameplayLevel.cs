#region

using System;
using System.Threading.Tasks;
using Data.Configurations.GameplayMode;

#endregion

namespace Core.Gameplay
{
    public interface IGameplayLevel
    {
        event Action<GameResult> OnGameFinished;

        Task StartGame<TGameplayModeData>(TGameplayModeData gameplayModeData)
            where TGameplayModeData : GameplayModeData;
    }

    public enum GameResult
    {
        Exit,
        Win,
        Lose,
        Error
    }
}