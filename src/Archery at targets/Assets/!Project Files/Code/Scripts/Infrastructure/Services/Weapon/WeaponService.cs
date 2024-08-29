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

        private Dictionary<string, WeaponData> _allWeaponsCache;
        private Dictionary<string, WeaponData> _ownedWeaponsCache;
        private WeaponData _currentWeaponData;

        public WeaponService(IStaticDataService staticDataService, IProgressService progressService)
        {
            _staticDataService = staticDataService;
            _progressService = progressService;
        }

        public void Initialize()
        {
            var progressData = _progressService.Get();
            
            CacheAllWeapons();
            
            CacheOwnedWeapons(progressData.unlockedWeapons);

            _currentWeaponData = string.IsNullOrEmpty(progressData.currentWeaponId)
                ? GetDefaultWeapon()
                : GetWeaponData(progressData.currentWeaponId);
        }

        public AssetReference GetCurrentlyEquippedWeaponReference() => _currentWeaponData.Reference;

        public WeaponData GetCurrentlyEquippedWeaponData() => _currentWeaponData;

        public WeaponData GetWeaponData(string weaponId)
        {
            if (_allWeaponsCache.TryGetValue(weaponId, out var weaponData))
            {
                return weaponData;
            }

            var errorMessage = $"Weapon with ID {weaponId} not found.";
            Debug.LogError(errorMessage);
            throw new ArgumentException(errorMessage, nameof(weaponId));
        }

        public IEnumerable<WeaponData> GetWeaponDatas(WeaponReceiptType? weaponReceiptType = null)
        {
            return weaponReceiptType.HasValue
                ? _allWeaponsCache.Values.Where(w => w.WeaponReceiptType == weaponReceiptType.Value)
                : _allWeaponsCache.Values;
        }

        public IEnumerable<WeaponData> GetOwnedWeaponDatas(WeaponReceiptType? weaponReceiptType = null)
        {
            return weaponReceiptType.HasValue
                ? _ownedWeaponsCache.Values.Where(w => w.WeaponReceiptType == weaponReceiptType.Value)
                : _ownedWeaponsCache.Values;
        }

        public void EquipWeapon(string weaponId)
        {
            _currentWeaponData = GetWeaponData(weaponId);

            var progressData = _progressService.Get();
            progressData.currentWeaponId = weaponId;
            _progressService.Set(progressData);
        }

        public void UnlockWeapon(string weaponId)
        {
            if (_allWeaponsCache.TryGetValue(weaponId, out var weaponData))
            {
                var progressData = _progressService.Get();
                if (!progressData.unlockedWeapons.Contains(weaponId))
                {
                    progressData.unlockedWeapons.Add(weaponId);
                    _progressService.Set(progressData);
                    _ownedWeaponsCache[weaponId] = weaponData;
                }
            }
            else
            {
                var errorMessage = $"Weapon with ID {weaponId} not found.";
                Debug.LogError(errorMessage);
                throw new ArgumentException(errorMessage, nameof(weaponId));
            }
        }

        private void CacheAllWeapons()
        {
            _allWeaponsCache = _staticDataService
                .GetWeaponData<WeaponData>()
                .ToDictionary(w => w.Key, w => w);
        }

        private void CacheOwnedWeapons(IEnumerable<string> unlockedWeaponIds)
        {
            _ownedWeaponsCache = _allWeaponsCache
                .Where(w => unlockedWeaponIds.Contains(w.Key))
                .ToDictionary(w => w.Key, w => w.Value);
        }

        private WeaponData GetDefaultWeapon()
        {
            var defaultWeapon =
                _allWeaponsCache.Values.FirstOrDefault(w => w.WeaponReceiptType == WeaponReceiptType.Default);

            if (!defaultWeapon)
            {
                const string ERROR_MESSAGE = "Default weapon not found in the static data.";
                Debug.LogError(ERROR_MESSAGE);
                throw new Exception(ERROR_MESSAGE);
            }

            return defaultWeapon;
        }
    }
}