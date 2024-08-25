using System.Threading.Tasks;
using Features.Projectile;
using UnityEngine;

namespace Infrastructure.Factories.Projectile
{
    public interface IProjectileFactory
    {
        Task<IProjectile> Instantiate(Transform parent = null);

        GameObject GetInstance(IProjectile projectile);

        void Destroy(IProjectile projectile);
    }
}