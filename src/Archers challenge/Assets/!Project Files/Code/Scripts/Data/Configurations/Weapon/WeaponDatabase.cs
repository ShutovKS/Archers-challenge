#region

using Data.Configurations.Database;
using UnityEngine;

#endregion

namespace Data.Configurations.Weapon
{
    [CreateAssetMenu(fileName = "WeaponDatabase", menuName = "Data/Weapon/Database")]
    public class WeaponDatabase : Database<WeaponData>
    {
    }
}