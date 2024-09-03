#region

using Infrastructure.Services.XRSetup;
using UnityEngine;

#endregion

namespace Data.Level
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "Data/Level/Base", order = 0)]
    public class LevelData : ScriptableObject
    {
        [field: SerializeField] public string Key { get; private set; } = System.Guid.NewGuid().ToString();

        [field: Header("Information")]
        [field: SerializeField] public string LevelName { get; private set; }
        [field: SerializeField] public string ShortDescription { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }

        [field: Header("Sounds")]
        [field: SerializeField] public AudioClip Music { get; private set; }
        [field: SerializeField, Range(0.0f, 1.0f)] public float MusicVolume { get; private set; } = 1.0f;

        [field: Header("References")]
        [field: SerializeField] public string LocationScenePath { get; private set; }

        [field: Header("Player Configure")]
        [field: SerializeField] public XRMode XRMode { get; private set; } = XRMode.VR;
    }
}