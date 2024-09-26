using Data.Constants.Paths;

namespace Infrastructure.Services.Projectile
{
    public class ProjectileService : IProjectileService
    {
        public string GetCurrentlySelectedProjectilePath()
        {
            return AddressablesPaths.ARROW_PREFAB;
        }
    }
}