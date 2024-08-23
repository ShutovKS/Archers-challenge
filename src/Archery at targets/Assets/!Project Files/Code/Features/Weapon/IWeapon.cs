using System;

namespace Features.Weapon
{
    public interface IWeapon
    {
        event Action<bool> OnSelected;
        event Action<bool> OnVisualizeProjectile;
        event Action<float> OnPullReleased;
        
        bool IsSelected { get; }
    }
}