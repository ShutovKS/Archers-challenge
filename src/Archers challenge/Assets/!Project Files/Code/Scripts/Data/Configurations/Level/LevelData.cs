#region

using Infrastructure.Services.XRSetup;
using UnityEngine;
using UnityEngine.AddressableAssets;

#endregion

namespace Data.Configurations.Level
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "Data/Level/Base", order = 0)]
    public class LevelData : ScriptableObject
    {
        [field: SerializeField] public string Key { get; private set; } = System.Guid.NewGuid().ToString();
        [field: SerializeField] public int LevelIndex { get; private set; }

        [field: Header("Information")]
        [field: SerializeField]
        public string LevelName { get; private set; }

        [field: SerializeField] public string ShortDescription { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }

        [field: Header("Sounds")]
        [field: SerializeField]
        public AudioClip Music { get; private set; }

        [field: SerializeField, Range(0.0f, 1.0f)]
        public float MusicVolume { get; private set; } = 1.0f;

        [field: Header("References")]
        [field: SerializeField]
        public AssetReference LocationScenePath { get; private set; }

        [field: Header("Game Configure")]
        [field: SerializeField]
        public XRMode XRMode { get; private set; } = XRMode.VR;

        [field: SerializeField] public bool IsGravityEnabled { get; private set; } = true;

        [Header("Gameplay Mode")] [SerializeReference]
        private GameplayModeData gameplayModeData;

        public GameplayModeData GameplayModeData => gameplayModeData;

        public void OnValidate()
        {
            LevelDatabase.Instance.OnValidate(this);
        }
    }
}