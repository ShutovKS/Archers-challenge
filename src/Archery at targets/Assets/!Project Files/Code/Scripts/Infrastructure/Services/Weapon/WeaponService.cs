using System;
using System.Collections.Generic;
using System.Linq;
using Data.Weapon;
using Infrastructure.Services.Progress;
using Infrastructure.Services.StaticData;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Infrastructure.Services.Weapon
{
    [UsedImplicitly]
    public class WeaponService : IWeaponService, IInitializable
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IProgressService _progressService;
        private Dictionary<string, WeaponData> _cachedWeapons;

        public WeaponService(IStaticDataService staticDataService, IProgressService progressService)
        {
            _staticDataService = staticDataService;
            _progressService = progressService;
        }

        public void Initialize()
        {
            _cachedWeapons = _staticDataService.GetWeaponData<WeaponData>().ToDictionary(w => w.Key, w => w);

            var progressData = _progressService.Get();

            if (string.IsNullOrEmpty(progressData.currentWeaponId))
            {
                EquipWeapon(GetDefaultWeapon().Key);
            }
        }

        public AssetReference GetCurrentlyEquippedWeaponReference()
        {
            var progressData = _progressService.Get();
            return GetWeaponData(progressData.currentWeaponId).Reference;
        }

        public WeaponData GetCurrentlyEquippedWeaponData()
        {
            var progressData = _progressService.Get();
            return GetWeaponData(progressData.currentWeaponId);
        }

        public WeaponData GetWeaponData(string weaponId)
        {
            if (_cachedWeapons.TryGetValue(weaponId, out var weaponData))
            {
                return weaponData;
            }

            Debug.LogError($"Weapon with ID {weaponId} not found.");
            throw new ArgumentException($"Weapon with ID {weaponId} not found.", nameof(weaponId));
        }

        public WeaponData[] GetWeaponDatas(WeaponReceiptType? weaponReceiptType = null)
        {
            return weaponReceiptType == null
                ? _cachedWeapons.Values.ToArray()
                : _cachedWeapons.Values.Where(w => w.WeaponReceiptType == weaponReceiptType).ToArray();
        }

        public void EquipWeapon(string weaponId)
        {
            var weaponData = GetWeaponData(weaponId); // Exception if a weapon not found
            _progressService.SetCurrentWeapon(weaponId);
        }

        private WeaponData GetDefaultWeapon()
        {
            var weaponDatas = _staticDataService.GetWeaponData<WeaponData>();

            foreach (var weaponData in weaponDatas)
            {
                if (weaponData.WeaponReceiptType == WeaponReceiptType.Default)
                {
                    return weaponData;
                }
            }

            Debug.LogError("Default weapon not found in the static data.");
            throw new Exception("Default weapon not found");
        }
    }
}