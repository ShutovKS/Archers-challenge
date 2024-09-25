#region

using Data.Configurations.Level;
using Data.Configurations.Weapon;

#endregion

namespace Infrastructure.Providers.StaticData
{
    public interface IStaticDataProvider
    {
        TLevelData[] GetLevelData<TLevelData>() where TLevelData : LevelData;
        TLevelData GetLevelData<TLevelData>(string key) where TLevelData : LevelData;

        TWeaponData[] GetWeaponData<TWeaponData>() where TWeaponData : WeaponData;
        TWeaponData GetWeaponData<TWeaponData>(string key) where TWeaponData : WeaponData;
    }
}