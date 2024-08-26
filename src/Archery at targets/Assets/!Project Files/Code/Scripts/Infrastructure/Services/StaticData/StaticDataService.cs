using System;
using System.Collections.Generic;
using System.Linq;
using Data.Level;
using Data.Path;
using Data.Weapon;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace Infrastructure.Services.StaticData
{
    [UsedImplicitly]
    public class StaticDataService : IStaticDataService, IInitializable
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

        public TLevelData GetLevelData<TLevelData>(string key) where TLevelData : LevelData
        {
            if (_levels.TryGetValue(key, out var levelData))
            {
                return (TLevelData)levelData;
            }

            throw new Exception($"No level data for key: {key}");
        }
    }
}