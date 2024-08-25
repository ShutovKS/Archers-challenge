using Data.Database;
using UnityEngine;

namespace Data.Level
{
    [CreateAssetMenu(fileName = "LevelDatabase", menuName = "Data/Level/Database", order = 0)]
    public class LevelDatabase : Database<LevelData> { }
}