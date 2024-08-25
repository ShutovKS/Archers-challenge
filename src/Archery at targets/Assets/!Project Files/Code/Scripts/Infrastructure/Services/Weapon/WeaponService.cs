using Infrastructure.Services.AssetsAddressables;

namespace Infrastructure.Services.Weapon
{
    public class WeaponService : IWeaponService
    {
        public string GetCurrentlyEquippedWeaponPath()
        {
            return AssetsAddressableConstants.BOW_PREFAB;
        }
    }
}