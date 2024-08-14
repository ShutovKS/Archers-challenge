using UnityEngine;

namespace Data.Level
{
    [CreateAssetMenu(fileName = "ShootingData", menuName = "Data/Level/Shooting", order = 0)]
    public class ShootingBaseLevelData : BaseLevelData
    {
        [field: SerializeField] public CreatedObjectData ScreenCreatedObjectData { get; private set; }
    }
}