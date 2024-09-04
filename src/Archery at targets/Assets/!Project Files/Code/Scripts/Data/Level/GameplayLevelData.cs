#region

using Data.Gameplay;
using UnityEngine;

#endregion

namespace Data.Level
{
    [CreateAssetMenu(fileName = "GameplayLevel", menuName = "Data/Level/Gameplay", order = 0)]
    public class GameplayLevelData : LevelData
    {
        [field: Header("Gameplay")]
        [field: SerializeField] public int LevelIndex { get; private set; }

        [field: SerializeReference] public GameplayModeData GameplayModeData { get; private set; }
    }
}