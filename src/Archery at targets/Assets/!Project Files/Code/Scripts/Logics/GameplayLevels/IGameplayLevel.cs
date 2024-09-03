#region

using System;
using System.Threading.Tasks;

#endregion

namespace Logics.GameplayLevels
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