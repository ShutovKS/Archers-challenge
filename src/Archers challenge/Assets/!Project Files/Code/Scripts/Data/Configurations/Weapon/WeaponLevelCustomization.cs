#region

using System;
using Tools;
using UnityEngine;
using UnityEngine.AddressableAssets;

#endregion

namespace Data.Configurations.Weapon
{
    [Serializable]
    public class WeaponLevelCustomization : WeaponCustomization<WeaponLevelCustomization.WeaponLevel>
    {
        public override void Validate()
        {
            base.Validate();

            foreach (var level in Customizations)
            {
                level.Validate();
            }
        }

        [Serializable]
        public class WeaponLevel : ICustomizable
        {
            [field: SerializeField] public string Key { get; private set; }
            [field: SerializeField] public int Level { get; private set; }
            [field: SerializeField] public AssetReference Reference { get; private set; }

            public void Validate()
            {
                if (Reference == null)
                {
                    Debug.LogWarning($"WeaponLevel: Reference is not assigned.");
                }

                if (Level <= 0)
                {
                    Debug.LogError($"WeaponLevel: Level must be greater than 0. Current level: {Level}");
                }
            }

            public void GenerateKey()
            {
                Key = KeyGenerator.GenerateKey();
            }
        }
    }
}