#region

using System.Collections.Generic;
using System.Threading.Tasks;
using Features.Weapon;
using Infrastructure.Factories.GameObjects;
using Infrastructure.Services.Weapon;
using JetBrains.Annotations;
using UnityEngine;

#endregion

namespace Infrastructure.Factories.Weapon
{
    [UsedImplicitly]
    public class WeaponFactory : IWeaponFactory
    {
        private readonly IWeaponService _weaponService;
        private readonly IGameObjectFactory _gameObjectFactory;
        private readonly Dictionary<IWeapon, GameObject> _weaponToGameObjectMap = new();

        public WeaponFactory(IWeaponService weaponService, IGameObjectFactory gameObjectFactory)
        {
            _weaponService = weaponService;
            _gameObjectFactory = gameObjectFactory;
        }

        public async Task<IWeapon> Instantiate(Vector3? position = null, Quaternion? rotation = null, Transform parent = null)
        {
            var weaponReference = _weaponService.GetCurrentlyEquippedWeaponReference();

            var instantiate = await _gameObjectFactory.InstantiateAsync(weaponReference, position, rotation, parent);

            var weapon = instantiate.GetComponent<IWeapon>();

            _weaponToGameObjectMap.Add(weapon, instantiate);

            return weapon;
        }

        public void Destroy(IWeapon weapon)
        {
            if (_weaponToGameObjectMap.Remove(weapon, out var gameObject))
            {
                _gameObjectFactory.Destroy(gameObject);
            }
        }
    }
}