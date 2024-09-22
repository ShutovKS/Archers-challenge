#region

using System;
using System.Collections.Generic;
using System.Linq;
using Data.Configurations.Level;
using Data.Configurations.Weapon;
using Data.Constants.Paths;
using UnityEngine;
using Zenject;

#endregion

namespace Infrastructure.Providers.StaticData
{
    public class StaticDataProvider : IStaticDataProvider, IInitializable
    {
        private Dictionary<string, LevelData> _levels;
        private Dictionary<string, WeaponData> _weapons;

        public void Initialize()
        {
            _levels = Resources
                .Load<LevelDatabase>(ResourcesPaths.LEVEL_DATABASE)
                .Items
                .ToDictionary(x => x.Key, x => x);

            _weapons = Resources
                .Load<WeaponDatabase>(ResourcesPaths.WEAPON_DATABASE)
                .Items
                .ToDictionary(x => x.Key, x => x);
        }

        public TLevelData[] GetLevelData<TLevelData>() where TLevelData : LevelData =>
            _levels.Values.OfType<TLevelData>().ToArray();

        public TLevelData GetLevelData<TLevelData>(string key) where TLevelData : LevelData =>
            GetData(key, _levels) as TLevelData;

        public TWeaponData[] GetWeaponData<TWeaponData>() where TWeaponData : WeaponData =>
            _weapons.Values.OfType<TWeaponData>().ToArray();

        public TWeaponData GetWeaponData<TWeaponData>(string key) where TWeaponData : WeaponData =>
            GetData(key, _weapons) as TWeaponData;

        private static TData GetData<TData>(string key, Dictionary<string, TData> data)
        {
            if (data.TryGetValue(key, out var levelData))
            {
                return levelData;
            }

            throw new Exception($"Data with key {key} not found in database {typeof(TData)}");
        }
    }
}