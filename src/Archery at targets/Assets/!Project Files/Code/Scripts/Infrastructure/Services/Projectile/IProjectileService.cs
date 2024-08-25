using Infrastructure.Services.AssetsAddressables;

namespace Infrastructure.Services.Projectile
{
    public interface IProjectileService
    {
        string GetCurrentlySelectedProjectilePath();
    }
    
    public class ProjectileService : IProjectileService
    {
        public string GetCurrentlySelectedProjectilePath()
        {
            return AssetsAddressableConstants.ARROW_PREFAB;
        }
    }
}