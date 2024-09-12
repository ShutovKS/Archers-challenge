#region

using System;
using System.Collections.Generic;
using System.Linq;
using Data.Weapon;
using Infrastructure.Services.Progress;
using Infrastructure.Services.StaticData;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

#endregion

namespace Infrastructure.Services.Weapon
{
    public class WeaponService : IWeaponService, IInitializable
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IProgressService _progressService;

        private Dictionary<string, WeaponData> _allWeaponsCache;
        private Dictionary<string, WeaponData> _ownedWeaponsCache;
        private WeaponData _currentWeaponData;
        private string _currentCustomizationId;

        public event Action<WeaponData> OnWeaponEquipped;
        public event Action<WeaponData> OnWeaponUnlocked;

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
            
            _currentWeaponData = !string.IsNullOrEmpty(progressData.currentWeaponId)
                ? GetWeaponData(progressData.currentWeaponId)
                : GetDefaultWeapon();
            
            _currentCustomizationId = progressData.currentCustomizationId;
        }

        public AssetReference GetCurrentlyEquippedWeaponReference()
        {
            switch (_currentWeaponData.Customization)
            {
                case WeaponDefaultCustomization weaponDefaultCustomization:
                    return weaponDefaultCustomization.Reference;
                case WeaponLevelCustomization weaponLevelCustomization:
                    return weaponLevelCustomization.Customizations
                        .FirstOrDefault(c => c.Key == _currentCustomizationId)
                        ?.Reference;
                case WeaponColorCustomization weaponColorCustomization:
                    return weaponColorCustomization.Customizations
                        .FirstOrDefault(c => c.Key == _currentCustomizationId)
                        ?.Reference;
                default: throw new ArgumentOutOfRangeException(
                    $"Unknown customization type: {_currentWeaponData.Customization.GetType()}");
            }
        }

        public WeaponData GetCurrentlyEquippedWeaponData() => _currentWeaponData;

        public WeaponData GetWeaponData(string weaponId)
        {
            if (_allWeaponsCache.TryGetValue(weaponId, out var weaponData))
                return weaponData;

            var errorMessage = $"Weapon with ID {weaponId} not found.";
           
            Debug.LogError(errorMessage);
            throw new ArgumentException(errorMessage, nameof(weaponId));
        }

        public IEnumerable<WeaponData> GetWeaponDatas() => _allWeaponsCache.Values;

        public IEnumerable<WeaponData> GetOwnedWeaponDatas() => _ownedWeaponsCache.Values;

        public void EquipWeapon(string weaponId, string customizationId = null)
        {
            if (_allWeaponsCache.TryGetValue(weaponId, out var weaponData))
            {
                if (!_ownedWeaponsCache.ContainsKey(weaponId))
                {
                    const string ERROR_MESSAGE = "Weapon not unlocked.";
                    
                    Debug.LogError(ERROR_MESSAGE);
                    throw new InvalidOperationException(ERROR_MESSAGE);
                }

                _currentWeaponData = weaponData;
                _currentCustomizationId = customizationId;
                
                var progressData = _progressService.Get();
                progressData.currentWeaponId = weaponId;
                progressData.currentCustomizationId = customizationId;
                
                _progressService.Set(progressData);
                
                OnWeaponEquipped?.Invoke(weaponData);
            }
            else
            {
                var errorMessage = $"Weapon with ID {weaponId} not found.";
                
                Debug.LogError(errorMessage);
                throw new ArgumentException(errorMessage, nameof(weaponId));
            }
        }

        public void UnlockWeapon(string weaponId, string customizationId = null)
        {
            if (_allWeaponsCache.TryGetValue(weaponId, out var weaponData))
            {
                if (_ownedWeaponsCache.TryAdd(weaponId, weaponData))
                {
                    var progressData = _progressService.Get();
                    progressData.unlockedWeapons.Add(weaponId);

                    _progressService.Set(progressData);

                    OnWeaponUnlocked?.Invoke(weaponData);
                }
                else
                {
                    const string ERROR_MESSAGE = "Weapon already unlocked.";

                    Debug.LogError(ERROR_MESSAGE);
                    throw new InvalidOperationException(ERROR_MESSAGE);
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
            var defaultWeapon = _allWeaponsCache.Values.FirstOrDefault(w => w.IsUnlocked);
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