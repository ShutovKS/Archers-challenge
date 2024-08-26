using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Data.Weapon
{
    [CreateAssetMenu(fileName = "WeaponData", menuName = "Data/Weapon/Data")]
    public class WeaponData : ScriptableObject
    {
        [field: SerializeField] public string Key { get; private set; } = System.Guid.NewGuid().ToString();

        [field: Header("Information")]
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }

        [field: Header("Stats")]
        [field: SerializeField] public bool IsUnlocked { get; private set; }
        [field: SerializeField] public int Price { get; private set; }

        [field: Header("References")]
        [field: SerializeField] public AssetReference Reference { get; private set; }
    }
}