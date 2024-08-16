using UnityEngine;

namespace Data.Level
{
    [CreateAssetMenu(fileName = "MainMenuData", menuName = "Data/Level/MainMenu", order = 0)]
    public class MainMenuLevelData : BaseLevelData
    {
        [field: SerializeField] public SpawnPoint ScreenSpawnPoint { get; private set; }
    }
}