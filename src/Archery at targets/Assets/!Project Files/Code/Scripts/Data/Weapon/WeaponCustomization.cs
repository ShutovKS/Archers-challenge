using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data.Weapon
{
    [Serializable]
    public abstract class WeaponCustomization<T> : WeaponCustomization where T : ICustomizable
    {
        [field: SerializeField] public T[] Customizations { get; private set; }

        public override void Validate()
        {
            if (Customizations == null || Customizations.Length == 0)
            {
                Debug.LogError($"WeaponCustomization<{typeof(T).Name}>: Customizations are null or empty. " +
                               $"Please assign at least one customization.");
                return;
            }

            var keys = new HashSet<string>();

            foreach (var customization in Customizations)
            {
                if (customization == null)
                {
                    Debug.LogError($"WeaponCustomization<{typeof(T).Name}>: A customization is null. " +
                                   $"Please ensure all customizations are assigned.");
                    continue;
                }

                customization.Validate();

                if (string.IsNullOrEmpty(customization.Key))
                {
                    customization.GenerateKey();
                    Debug.LogWarning($"WeaponCustomization<{typeof(T).Name}>: Customization key was empty, " +
                                     $"generated a new one: {customization.Key}");
                }

                if (!keys.Add(customization.Key))
                {
                    customization.GenerateKey();
                    Debug.LogWarning($"WeaponCustomization<{typeof(T).Name}>: Duplicate key detected." +
                                     $" Generated a new key: {customization.Key}");
                    keys.Add(customization.Key);
                }
            }
        }
    }

    [Serializable]
    public abstract class WeaponCustomization
    {
        public abstract void Validate();
    }
}