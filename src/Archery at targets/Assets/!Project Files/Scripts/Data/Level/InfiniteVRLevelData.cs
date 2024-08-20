using UnityEngine;

namespace Data.Level
{
    [CreateAssetMenu(fileName = "InfiniteVRData", menuName = "Data/Level/InfiniteVR", order = 0)]
    public class InfiniteVRLevelData : BaseLevelData
    {
        public override string Key { get; protected set; } = "InfiniteVR";
        [field: SerializeField] public SpawnPoint InfoScreenSpawnPoint { get; private set; }
        [field: SerializeField] public string InfoScreenPrefabPath { get; private set; }
    }
}