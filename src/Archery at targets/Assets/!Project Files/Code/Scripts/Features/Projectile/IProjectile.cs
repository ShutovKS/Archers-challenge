using System;

namespace Features.Projectile
{
    public interface IProjectile
    {
        event Action OnStopped;

        void Fire(float pullAmount);
    }
}