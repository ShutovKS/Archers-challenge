#region

using System.Collections.Generic;
using Data.Weapon;
using UnityEngine.AddressableAssets;

#endregion

namespace Infrastructure.Services.Weapon
{
    public interface IWeaponService
    {
        AssetReference GetCurrentlyEquippedWeaponReference();
        WeaponData GetCurrentlyEquippedWeaponData();
        WeaponData GetWeaponData(string weaponId);
        IEnumerable<WeaponData> GetWeaponDatas();
        IEnumerable<WeaponData> GetOwnedWeaponDatas();
        void EquipWeapon(string weaponId, string customizationId = null);
        void UnlockWeapon(string weaponId, string customizationId = null);
    }
}