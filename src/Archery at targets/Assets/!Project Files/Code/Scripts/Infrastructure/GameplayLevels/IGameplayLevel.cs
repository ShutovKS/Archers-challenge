#region

using System;
using System.Threading.Tasks;

#endregion

namespace Infrastructure.GameplayLevels
{
    public interface IGameplayLevel
    {
        event Action<GameResult> OnGameFinished;

        Task StartGame();
    }

    public enum GameResult
    {
        Exit,
        Win,
        Lose,
    }
}