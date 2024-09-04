#region

using Data.Paths;

#endregion

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
            return AddressablesPaths.ARROW_PREFAB;
        }
    }
}