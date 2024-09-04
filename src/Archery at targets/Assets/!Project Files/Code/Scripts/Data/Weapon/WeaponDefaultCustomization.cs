#region

using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

#endregion

namespace Data.Weapon
{
    [Serializable]
    public class WeaponDefaultCustomization : WeaponCustomization
    {
        [field: SerializeField] public AssetReference Reference { get; private set; }

        public override void Validate()
        {
            if (Reference == null)
            {
                Debug.LogWarning($"WeaponDefaultCustomization: Reference is not assigned.");
            }
        }
    }
}