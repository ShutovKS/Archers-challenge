using System.Collections.Generic;
using System.Threading.Tasks;
using Features.Projectile;
using Infrastructure.Factories.GameObjects;
using Infrastructure.Services.Projectile;
using Tools;
using UnityEngine;

namespace Infrastructure.Factories.Projectile
{
    public interface IProjectileFactory
    {
        Task<string> Instantiate(Transform parent = null);

        IProjectile GetProjectile(string projectileId);

        void Destroy(string projectileId);
    }

    public class ProjectileFactory : IProjectileFactory
    {
        private readonly IProjectileService _projectileService;
        private readonly IGameObjectFactory _gameObjectFactory;
        private readonly Dictionary<string, GameObject> _uniqueIdToInstance = new();
        private readonly Dictionary<string, IProjectile> _uniqueIdToProjectile = new();

        public ProjectileFactory(IProjectileService projectileService, IGameObjectFactory gameObjectFactory)
        {
            _projectileService = projectileService;
            _gameObjectFactory = gameObjectFactory;
        }

        public async Task<string> Instantiate(Transform parent = null)
        {
            var path = _projectileService.GetCurrentlySelectedProjectilePath();

            var gameObject = await _gameObjectFactory.Instantiate(path, parent: parent);
            var projectileId = UniqueIDGenerator.Generate();

            _uniqueIdToInstance.Add(projectileId, gameObject);
            _uniqueIdToProjectile.Add(projectileId, gameObject.GetComponent<IProjectile>());

            return projectileId;
        }

        public IProjectile GetProjectile(string projectileId)
        {
            return _uniqueIdToProjectile[projectileId];
        }

        public void Destroy(string projectileId)
        {
            _uniqueIdToProjectile.Remove(projectileId);

            if (_uniqueIdToInstance.Remove(projectileId, out var value))
            {
                _gameObjectFactory.Destroy(value);
            }
        }
    }
}