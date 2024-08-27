using Data.Level;
using Data.Weapon;

namespace Infrastructure.Services.StaticData
{
    public interface IStaticDataService
    {
        TLevelData[] GetLevelData<TLevelData>() where TLevelData : LevelData;
        TLevelData GetLevelData<TLevelData>(string key) where TLevelData : LevelData;
        
        TWeaponData[] GetWeaponData<TWeaponData>() where TWeaponData : WeaponData;
        TWeaponData GetWeaponData<TWeaponData>(string key) where TWeaponData : WeaponData;
    }
}