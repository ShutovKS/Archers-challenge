#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Configurations.Weapon;
using Features.Weapon;
using Infrastructure.Factories.GameObjects;
using Infrastructure.Providers.StaticData;
using Infrastructure.Services.Progress;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

#endregion

namespace Infrastructure.Services.Weapon
{
    public class WeaponService : IWeaponService, IInitializable
    {
        private readonly IStaticDataProvider _staticDataProvider;
        private readonly IProgressService _progressService;
        private readonly IGameObjectFactory _gameObjectFactory;

        private Dictionary<string, WeaponData> _allWeaponsCache;
        private Dictionary<string, WeaponData> _ownedWeaponsCache;
        private WeaponData _currentWeaponData;
        private string _currentCustomizationId;
        private GameObject _currentWeaponInstance;
        private Rigidbody _currentWeaponRigidbody;

        public IWeapon CurrentWeapon { get; private set; }
        public event Action<WeaponData> OnWeaponEquipped;
        public event Action<WeaponData> OnWeaponUnlocked;

        public WeaponService(IStaticDataProvider staticDataProvider, IProgressService progressService,
            IGameObjectFactory gameObjectFactory)
        {
            _staticDataProvider = staticDataProvider;
            _progressService = progressService;
            _gameObjectFactory = gameObjectFactory;
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

        public async Task InstantiateEquippedWeapon(Vector3 position, Quaternion rotation)
        {
            var instance = await _gameObjectFactory.InstantiateAsync(
                GetCurrentlyEquippedWeaponReference(),
                position,
                rotation
            );

            _currentWeaponInstance = instance;
            CurrentWeapon = instance.GetComponent<IWeapon>();

            _currentWeaponRigidbody = instance.GetComponent<Rigidbody>();
        }

        public void DestroyWeapon()
        {
            if (_currentWeaponInstance)
            {
                _gameObjectFactory.Destroy(_currentWeaponInstance);

                _currentWeaponInstance = null;

                CurrentWeapon = null;
            }
        }

        public void SetActiveGravities(bool active) => _currentWeaponRigidbody.useGravity = active;

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
            _allWeaponsCache = _staticDataProvider
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

        private AssetReference GetCurrentlyEquippedWeaponReference() => _currentWeaponData.Customization switch
        {
            WeaponDefaultCustomization weaponDefaultCustomization => weaponDefaultCustomization.Reference,
            WeaponLevelCustomization weaponLevelCustomization => weaponLevelCustomization.Customizations
                .FirstOrDefault(c => c.Key == _currentCustomizationId)?.Reference,
            WeaponColorCustomization weaponColorCustomization => weaponColorCustomization.Customizations
                .FirstOrDefault(c => c.Key == _currentCustomizationId)?.Reference,
            _ => throw new ArgumentOutOfRangeException(
                $"Unknown customization type: {_currentWeaponData.Customization.GetType()}")
        };
    }
}