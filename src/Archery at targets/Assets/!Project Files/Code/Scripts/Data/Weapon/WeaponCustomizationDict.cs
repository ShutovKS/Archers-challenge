#region

using System;
using System.Collections.Generic;
using UnityEngine;

#endregion

namespace Data.Weapon
{
    [Serializable]
    public abstract class WeaponCustomizationDict<T> : WeaponCustomization where T : ICustomizable
    {
        [field: SerializeField] private List<T> customizations = new();

        private Dictionary<string, T> _customizationDict = new();

        public IReadOnlyDictionary<string, T> Customizations => _customizationDict;

        public override void Validate()
        {
            _customizationDict.Clear();
            var keys = new HashSet<string>();

            foreach (var customization in customizations)
            {
                if (customization == null)
                {
                    Debug.LogError($"WeaponCustomizationDict<{typeof(T).Name}>: A customization is null. " +
                                   $"Please ensure all customizations are assigned.");
                    continue;
                }

                customization.Validate();

                if (string.IsNullOrEmpty(customization.Key))
                {
                    customization.GenerateKey();
                    Debug.LogWarning($"WeaponCustomizationDict<{typeof(T).Name}>: Customization key was empty, " +
                                     $"generated a new one: {customization.Key}");
                }

                if (!keys.Add(customization.Key))
                {
                    customization.GenerateKey();
                    Debug.LogWarning($"WeaponCustomizationDict<{typeof(T).Name}>: Duplicate key detected. " +
                                     $"Generated a new key: {customization.Key}");
                    keys.Add(customization.Key);
                }

                _customizationDict[customization.Key] = customization;
            }
        }
    }
}