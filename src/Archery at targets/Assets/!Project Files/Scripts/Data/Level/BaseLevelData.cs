using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Data.Level
{
    public abstract class BaseLevelData : ScriptableObject
    {
        public abstract string Key { get; protected set; }
        
        [field: Header("Information")]
        [field: SerializeField] public string LevelName { get; private set; }
        [field: SerializeField] public string ShortDescription { get; private set; }
        
        [field: Header("Sounds")]
        [field: SerializeField] public AudioClip Music { get; private set; }
        [field: SerializeField, Range(0.0f, 1.0f)] public float MusicVolume { get; private set; }
        
        [field: Header("References")]
        [field: SerializeField] public AssetReference LocationSceneReference { get; private set; }
        [field: SerializeField] public SpawnPoint PlayerSpawnPoint { get; private set; }
    }
}