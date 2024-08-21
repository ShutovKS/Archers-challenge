using UnityEngine;

namespace Data.Level
{
    [CreateAssetMenu(fileName = "InfiniteMRData", menuName = "Data/Level/InfiniteMR", order = 0)]
    public class InfiniteMRLevelData : BaseLevelData
    {
        public override string Key { get; protected set; } = "InfiniteMR";
        [field: SerializeField] public SpawnObject InfoScreenSpawnObject { get; private set; }
    }
}