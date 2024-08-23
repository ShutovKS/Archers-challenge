using System;

namespace Features.Weapon
{
    public interface IWeapon
    {
        event Action<bool> OnSelected;
        event Action<float> OnPullReleased;
    }
}