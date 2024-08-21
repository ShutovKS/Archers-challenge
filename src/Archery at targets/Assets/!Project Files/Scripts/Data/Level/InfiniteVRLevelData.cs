using UnityEngine;

namespace Data.Level
{
    [CreateAssetMenu(fileName = "InfiniteVRData", menuName = "Data/Level/InfiniteVR", order = 0)]
    public class InfiniteVRLevelData : BaseLevelData
    {
        public override string Key { get; protected set; } = "InfiniteVR";
        [field: SerializeField] public SpawnObject InfoScreenSpawnObject { get; private set; }
    }
}