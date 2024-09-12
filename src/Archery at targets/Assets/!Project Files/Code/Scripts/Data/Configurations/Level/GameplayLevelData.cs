#region

using Data.Configurations.GameplayMode;
using UnityEngine;

#endregion

namespace Data.Configurations.Level
{
    [CreateAssetMenu(fileName = "GameplayLevel", menuName = "Data/Level/Gameplay", order = 0)]
    public class GameplayLevelData : LevelData
    {
        [field: Header("Gameplay")]
        [field: SerializeField] public int LevelIndex { get; private set; }

        [SerializeReference] private GameplayModeData gameplayModeData;
        public GameplayModeData GameplayModeData => gameplayModeData;
    }
}