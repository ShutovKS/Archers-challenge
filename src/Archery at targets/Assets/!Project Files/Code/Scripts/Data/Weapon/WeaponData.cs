#region

using Tools;
using UnityEngine;

#endregion

namespace Data.Weapon
{
    [CreateAssetMenu(fileName = "WeaponData", menuName = "Data/Weapon/Data")]
    public class WeaponData : ScriptableObject
    {
        [field: SerializeField] public string Key { get; private set; }

        [Header("Information")]
        [field: SerializeField] public string Name { get; private set; } = "New Weapon";
        [field: SerializeField] public string Description { get; private set; } = "New Weapon Description";
        [field: SerializeField] public Sprite Icon { get; private set; }

        [Header("Stats")]
        [field: SerializeField] public bool IsUnlocked { get; private set; } = false;

        [Header("References")]
        [field: SerializeReference] public WeaponCustomization Customization { get; private set; }

        private void OnValidate()
        {
            ValidateKey();
            ValidateIcon();
            ValidateCustomization();
        }

        private void ValidateKey()
        {
            if (string.IsNullOrEmpty(Key))
            {
                Key = KeyGenerator.GenerateKey();

                Debug.LogWarning($"WeaponData: Key was empty, generated a new one: {Key}");
            }
        }

        private void ValidateIcon()
        {
            if (Icon == null)
            {
                Debug.LogWarning($"WeaponData: Icon is not assigned.");
            }
        }

        private void ValidateCustomization()
        {
            if (Customization == null)
            {
                Debug.LogError($"WeaponData: Customization is null. Please assign a valid customization.");
                return;
            }

            Customization.Validate();
        }
    }
}