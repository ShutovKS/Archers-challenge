using System.Collections.Generic;
using System.Threading.Tasks;
using Features.Projectile;
using Infrastructure.Factories.GameObjects;
using Infrastructure.Services.Projectile;
using JetBrains.Annotations;
using UnityEngine;

namespace Infrastructure.Factories.Projectile
{
    [UsedImplicitly]
    public class ProjectileFactory : IProjectileFactory
    {
        private readonly IGameObjectFactory _gameObjectFactory;
        private readonly IProjectileService _projectileProviderService;
        private readonly Dictionary<IProjectile, GameObject> _projectilesToInstantiates = new();

        public ProjectileFactory(IGameObjectFactory gameObjectFactory, IProjectileService projectileProviderService)
        {
            _gameObjectFactory = gameObjectFactory;
            _projectileProviderService = projectileProviderService;
        }

        public async Task<IProjectile> Instantiate(Transform parent = null)
        {
            var path = _projectileProviderService.GetCurrentlySelectedProjectilePath();

            var instantiate = await _gameObjectFactory.Instantiate(path, parent: parent);
            instantiate.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            instantiate.transform.localScale = Vector3.one;

            var projectile = instantiate.GetComponent<IProjectile>();

            _projectilesToInstantiates.Add(projectile, instantiate);

            return projectile;
        }

        public GameObject GetInstance(IProjectile projectile)
        {
            return _projectilesToInstantiates[projectile];
        }

        public void Destroy(IProjectile projectile)
        {
            if (_projectilesToInstantiates.Remove(projectile, out var gameObject))
            {
                _gameObjectFactory.Destroy(gameObject);
            }
        }
    }
}