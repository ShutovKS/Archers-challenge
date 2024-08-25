using Data.Paths;

namespace Infrastructure.Services.Weapon
{
    public class WeaponService : IWeaponService
    {
        public string GetCurrentlyEquippedWeaponPath()
        {
            return AddressablesPaths.BOW_PREFAB;
        }
    }
}