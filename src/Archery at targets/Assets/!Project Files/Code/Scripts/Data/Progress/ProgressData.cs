using System;
using System.Collections.Generic;

namespace Data.Progress
{
    [Serializable]
    public class ProgressData
    {
        public bool isTutorialCompleted;

        public int coins;
        public List<string> unlockedWeapons = new();

        public string currentWeaponId;

        public List<LevelProgressData> levelProgressDataList = new();

        public static ProgressData CreateDefault() => new()
        {
            isTutorialCompleted = false,
            coins = 0,
            unlockedWeapons = new List<string>(),
            currentWeaponId = null,
            levelProgressDataList = new List<LevelProgressData>()
        };
    }

    [Serializable]
    public class LevelProgressData
    {
        public string levelId;
        public int stars;
    }
}