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
    }

    [Serializable]
    public class LevelProgressData
    {
        public string levelId;
        public int stars;
    }
}