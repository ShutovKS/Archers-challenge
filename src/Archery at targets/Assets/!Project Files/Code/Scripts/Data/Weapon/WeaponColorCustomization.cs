using System;
using Tools;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Data.Weapon
{
    [Serializable]
    public class WeaponColorCustomization : WeaponCustomization<WeaponColorCustomization.WeaponColor>
    {
        public override void Validate()
        {
            base.Validate();

            foreach (var color in Customizations)
            {
                color.Validate();
            }
        }

        public void GenerateKey()
        {
            Debug.LogWarning("WeaponColorCustomization: GenerateKey() is not implemented.");
        }

        [Serializable]
        public class WeaponColor : ICustomizable
        {
            [field: SerializeField] public string Key { get; private set; } = KeyGenerator.GenerateKey();

            [field: SerializeField] public Color Color { get; private set; }
            [field: SerializeField] public AssetReference Reference { get; private set; }

            public void Validate()
            {
                if (Reference == null)
                {
                    Debug.LogWarning($"WeaponColor: Reference is not assigned.");
                }
            }

            public void GenerateKey()
            {
                Key = KeyGenerator.GenerateKey();
            }
        }
    }
}