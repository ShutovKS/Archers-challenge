using Core.Gameplay;
using Data.Configurations.Level;

namespace Data.Contexts.Global
{
    public class GlobalContextData
    {
        public LevelData LevelData { get; set; }
        public IGameplayLevel GameplayLevel { get; set; }
    }
}