using System.Threading.Tasks;
using Features.Weapon;
using UnityEngine;

namespace Infrastructure.Factories.Weapon
{
    public interface IWeaponFactory
    {
        Task<IWeapon> Instantiate(Vector3? position = null, Quaternion? rotation = null, Transform parent = null);

        void Destroy(IWeapon weapon);
    }
}