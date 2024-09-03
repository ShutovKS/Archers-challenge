#region

using Data.Database;
using UnityEngine;

#endregion

namespace Data.Weapon
{
    [CreateAssetMenu(fileName = "WeaponDatabase", menuName = "Data/Weapon/Database")]
    public class WeaponDatabase : Database<WeaponData> { }
}