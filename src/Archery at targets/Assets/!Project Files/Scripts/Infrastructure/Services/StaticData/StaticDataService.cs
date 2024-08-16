using System;
using System.Collections.Generic;
using System.Linq;
using Data.Level;
using JetBrains.Annotations;
using UnityEngine;

namespace Infrastructure.Services.StaticData
{
    [UsedImplicitly]
    public class StaticDataService : IStaticDataService
    {
        private const string LEVEL_STATIC_DATA_PATH = "Data/Levels/";

        private Dictionary<string, BaseLevelData> _levels;

        public void Initialize()
        {
            _levels = Resources
                .LoadAll<BaseLevelData>(LEVEL_STATIC_DATA_PATH)
                .ToDictionary(x => x.Key, x => x);

            Debug.Log(_levels.Count);
        }

        public TLevelData GetLevelData<TLevelData>(string key) where TLevelData : BaseLevelData
        {
            if (_levels.TryGetValue(key, out var levelData))
            {
                return (TLevelData)levelData;
            }

            throw new Exception($"No level data for key: {key}");
        }
    }
}