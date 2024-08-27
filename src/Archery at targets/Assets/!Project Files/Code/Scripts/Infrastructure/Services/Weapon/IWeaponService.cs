using UnityEngine.AddressableAssets;

namespace Infrastructure.Services.Weapon
{
    public interface IWeaponService
    {
        AssetReference GetCurrentlyEquippedWeaponReference();
    }
}