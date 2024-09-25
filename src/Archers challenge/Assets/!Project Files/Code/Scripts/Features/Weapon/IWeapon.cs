#region

using System;
using Infrastructure.Factories.Projectile;

#endregion

namespace Features.Weapon
{
    public interface IWeapon
    {
        event Action<bool> OnSelected;

        void SetUp(IProjectileFactory projectileFactory, float bowForce);

        bool IsSelected { get; }

        void Fire(float pullAmount);
        void Charge();
        void Discharge();
    }
}