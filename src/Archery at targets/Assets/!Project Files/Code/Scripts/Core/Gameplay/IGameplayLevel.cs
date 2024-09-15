#region

using System;
using System.Threading.Tasks;
using Data.Configurations.Level;

#endregion

namespace Core.Gameplay
{
    public interface IGameplayLevel
    {
        event Action<GameResult> OnGameFinished;
        event Action<GameState> OnGameStateChanged;

        Task PrepareGame<TGameplayModeData>(TGameplayModeData gameplayModeData)
            where TGameplayModeData : GameplayModeData;

        Task StartGame();

        void PauseGame();
        void ResumeGame();
        Task StopGame();
        void CleanUp();
    }

    public enum GameState
    {
        NotStarted,
        Running,
        Paused,
        Finished
    }

    public enum GameResult
    {
        Win,
        Lose,
        Error,
        Aborted
    }
}