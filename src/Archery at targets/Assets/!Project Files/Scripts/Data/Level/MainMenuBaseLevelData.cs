using Infrastructure.Services.AssetsAddressables;
using UnityEngine;

namespace Data.Level
{
    [CreateAssetMenu(fileName = "MainMenuData", menuName = "Data/Level/MainMenu", order = 0)]
    public class MainMenuBaseLevelData : BaseLevelData
    {
        [field: SerializeField] public CreatedObjectData ScreenCreatedObjectData { get; private set; }
    }
}