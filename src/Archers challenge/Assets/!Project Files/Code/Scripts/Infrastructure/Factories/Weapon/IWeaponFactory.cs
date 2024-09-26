using System.Threading.Tasks;
using Features.Weapon;
using Infrastructure.Factories.GameObjects;
using Infrastructure.Factories.Projectile;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Infrastructure.Factories.Weapon
{
    public interface IWeaponFactory
    {
        Task<IWeapon> CreateWeaponAsync(AssetReference weaponReference, Vector3 position, Quaternion rotation,
            bool useGravity, float bowForce);

        void DestroyWeapon();
    }

    public class WeaponFactory : IWeaponFactory
    {
        private readonly IGameObjectFactory _gameObjectFactory;
        private readonly IProjectileFactory _projectileFactory;
        private GameObject _currentWeaponInstance;

        public WeaponFactory(IGameObjectFactory gameObjectFactory, IProjectileFactory projectileFactory)
        {
            _gameObjectFactory = gameObjectFactory;
            _projectileFactory = projectileFactory;
        }

        public async Task<IWeapon> CreateWeaponAsync(AssetReference weaponReference, Vector3 position,
            Quaternion rotation, bool useGravity, float bowForce)
        {
            _currentWeaponInstance = await _gameObjectFactory.InstantiateAsync(weaponReference, position, rotation);
            _currentWeaponInstance.GetComponent<Rigidbody>().useGravity = useGravity;

            var weapon = _currentWeaponInstance.GetComponent<IWeapon>();
            weapon.SetUp(_projectileFactory, bowForce);

            return weapon;
        }

        public void DestroyWeapon()
        {
            if (_currentWeaponInstance != null)
            {
                Object.Destroy(_currentWeaponInstance);
            }
        }
    }
}