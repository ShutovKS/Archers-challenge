using System;

namespace Features.Weapon
{
    public interface IWeapon
    {
        event Action<bool> OnSelected;

        bool IsSelected { get; }

        void Fire(float pullAmount);
        void Charge();
        void Discharge();
    }
}