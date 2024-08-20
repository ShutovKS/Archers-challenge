using UnityEngine;

namespace Data.Level
{
    [CreateAssetMenu(fileName = "GameplayLevel", menuName = "Data/Level/GameplayLevel", order = 0)]
    public class GameplayLevelData : BaseLevelData
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
        [field: SerializeField] public GameObject LevelPrefab { get; private set; }
    }
}