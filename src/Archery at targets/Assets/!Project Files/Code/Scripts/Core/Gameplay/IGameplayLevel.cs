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

        Task StartGame<TGameplayModeData>(TGameplayModeData gameplayModeData)
            where TGameplayModeData : GameplayModeData;

        void Dispose();
    }

    public enum GameResult
    {
        Win,
        Lose,
        Error
    }
}