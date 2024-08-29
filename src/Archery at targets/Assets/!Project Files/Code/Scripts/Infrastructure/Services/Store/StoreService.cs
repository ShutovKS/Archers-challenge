using System.Collections.Generic;
using System.Linq;
using Data.Weapon;
using Infrastructure.Services.Wallet;
using Infrastructure.Services.Weapon;
using JetBrains.Annotations;
using Zenject;

namespace Infrastructure.Services.Store
{
    [UsedImplicitly]
    public class StoreService : IStoreService, IInitializable
    {
        private readonly IWeaponService _weaponService;
        private readonly WalletService _walletService;

        private IEnumerable<WeaponData> _weapons;

        public StoreService(IWeaponService weaponService, WalletService walletService)
        {
            _weaponService = weaponService;
            _walletService = walletService;
        }

        public void Initialize()
        {
            _weapons = _weaponService.GetWeaponDatas(WeaponReceiptType.Shop);
        }

        public IEnumerable<WeaponData> GetWeapons() => _weapons;

        public IEnumerable<string> GetOwnedWeaponIds() => _weaponService.GetOwnedWeaponDatas(WeaponReceiptType.Shop)
            .Select(w => w.Key);

        public bool TryPurchaseWeapon(string weaponId)
        {
            var weapon = _weaponService.GetWeaponData(weaponId);
            if (weapon == null)
            {
                return false;
            }

            if (!_walletService.RemoveCoins(weapon.Price))
            {
                return false;
            }

            _weaponService.UnlockWeapon(weaponId);
            return true;
        }
    }
}