#region

using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Configurations.Weapon;
using Features.Weapon;
using UnityEngine;
using UnityEngine.AddressableAssets;

#endregion

namespace Infrastructure.Services.Weapon
{
    public interface IWeaponService
    {
        IWeapon CurrentWeapon { get; }
        Task InstantiateEquippedWeapon(Vector3 position, Quaternion rotation);
        void DestroyWeapon();

        WeaponData GetCurrentlyEquippedWeaponData();
        WeaponData GetWeaponData(string weaponId);

        IEnumerable<WeaponData> GetWeaponDatas();
        IEnumerable<WeaponData> GetOwnedWeaponDatas();

        void EquipWeapon(string weaponId, string customizationId = null);
        void UnlockWeapon(string weaponId, string customizationId = null);
    }
}