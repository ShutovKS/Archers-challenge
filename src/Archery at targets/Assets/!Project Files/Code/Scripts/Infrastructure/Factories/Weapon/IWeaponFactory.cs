using System.Threading.Tasks;
using Data.Configurations.Weapon;
using Data.Contexts.Scene;
using Features.Weapon;
using Infrastructure.Factories.GameObjects;
using Infrastructure.Factories.Projectile;
using Infrastructure.Providers.SceneContainer;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Infrastructure.Factories.Weapon
{
    public interface IWeaponFactory
    {
        Task<IWeapon> CreateWeaponAsync(AssetReference weaponAssetReference, Vector3 position, Quaternion rotation);
    }

    public class WeaponFactory : IWeaponFactory
    {
        private readonly IGameObjectFactory _gameObjectFactory;
        private readonly IProjectileFactory _projectileFactory;
        private readonly ISceneContextProvider _sceneContextProvider;

        public WeaponFactory(IGameObjectFactory gameObjectFactory, IProjectileFactory projectileFactory,
            ISceneContextProvider sceneContextProvider)
        {
            _gameObjectFactory = gameObjectFactory;
            _projectileFactory = projectileFactory;
            _sceneContextProvider = sceneContextProvider;
        }

        public async Task<IWeapon> CreateWeaponAsync(AssetReference weaponAssetReference, Vector3 position,
            Quaternion rotation)
        {
            var weaponGameObject = await _gameObjectFactory.InstantiateAsync(weaponAssetReference, position, rotation);
            var weapon = weaponGameObject.GetComponent<IWeapon>();

            weapon.SetUp(_projectileFactory, _sceneContextProvider.Get<GameplaySceneContextData>().BowForce);

            return weapon;
        }
    }
}