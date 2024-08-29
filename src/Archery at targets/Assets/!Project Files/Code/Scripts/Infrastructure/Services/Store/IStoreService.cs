using System.Collections.Generic;
using Data.Weapon;

namespace Infrastructure.Services.Store
{
    public interface IStoreService
    {
        IEnumerable<WeaponData> GetWeapons();
        IEnumerable<string> GetOwnedWeaponIds();
        bool TryPurchaseWeapon(string weaponId);
    }
}