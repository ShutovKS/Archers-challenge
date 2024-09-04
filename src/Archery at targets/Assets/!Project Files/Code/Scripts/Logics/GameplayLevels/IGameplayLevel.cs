#region

using System;
using System.Threading.Tasks;
using Data.Gameplay;

#endregion

namespace Logics.GameplayLevels
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
    }
}