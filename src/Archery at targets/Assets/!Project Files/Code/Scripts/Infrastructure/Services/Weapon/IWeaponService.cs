using System.Collections.Generic;
using Data.Weapon;
using UnityEngine.AddressableAssets;

namespace Infrastructure.Services.Weapon
{
    public interface IWeaponService
    {
        AssetReference GetCurrentlyEquippedWeaponReference();
        WeaponData GetCurrentlyEquippedWeaponData();
        WeaponData GetWeaponData(string weaponId);
        IEnumerable<WeaponData> GetWeaponDatas(WeaponReceiptType? weaponReceiptType = null);
        IEnumerable<WeaponData> GetOwnedWeaponDatas(WeaponReceiptType? weaponReceiptType = null);
        void EquipWeapon(string weaponId);
        void UnlockWeapon(string weaponId);
    }
}